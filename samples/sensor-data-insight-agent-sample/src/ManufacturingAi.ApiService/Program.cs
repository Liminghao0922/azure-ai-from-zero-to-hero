using Azure;
using Azure.AI.Agents.Persistent;
using ManufacturingAi.ApiService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AiFoundryOptions>(builder.Configuration.GetSection("AiFoundry"));

// Add memory cache for session management
builder.Services.AddMemoryCache();

// Register AgentService as Singleton to cache the agent and client
builder.Services.AddSingleton<AgentService>();

var app = builder.Build();

app.MapGet(
    "/health",
    ([FromServices] IOptions<AiFoundryOptions> options) =>
    {
        var configured =
            !string.IsNullOrWhiteSpace(options.Value.EndPointUrl)
            && !string.IsNullOrWhiteSpace(options.Value.AgentId);

        var details = new
        {
            status = configured ? "Healthy" : "Unhealthy",
            timestamp = DateTime.UtcNow,
            components = new
            {
                configuration = new
                {
                    endpointConfigured = !string.IsNullOrWhiteSpace(options.Value.EndPointUrl),
                    agentConfigured = !string.IsNullOrWhiteSpace(options.Value.AgentId),
                },
            },
        };

        return Results.Json(details);
    }
);

app.MapPost(
    "/analyze",
    async ([FromServices] AgentService agentService, [FromBody] AnalyzeRequest req) =>
    {
        // Use cached agent and agentsClient from singleton service
        var agentsClient = agentService.AgentsClient;
        var agent = agentService.Agent;

        // Get or create session with thread
        var session = agentService.GetOrCreateSession(req.SessionId, req.RawData);
        var threadId = session.ThreadId;

        Console.WriteLine($"Using thread {threadId} for session {req.SessionId}");

        var userPrompt =
            req.UserPrompt
            ?? "Analyze the following manufacturing data and provide insights in Japanese.";

        // If this is the first message with data context, include raw data
        string messageContent;
        if (!session.HasDataContext && !string.IsNullOrWhiteSpace(req.RawData))
        {
            messageContent =
                $"{userPrompt}{Environment.NewLine}{Environment.NewLine}Manufacturing Data:{Environment.NewLine}{req.RawData}";
            session.HasDataContext = true;
            Console.WriteLine($"Sending data context for session {req.SessionId}");
        }
        else
        {
            messageContent = userPrompt;
            Console.WriteLine($"Sending query only for session {req.SessionId}");
        }

        PersistentThreadMessage messageResponse = agentsClient.Messages.CreateMessage(
            threadId,
            MessageRole.User,
            messageContent
        );

        ThreadRun run = agentsClient.Runs.CreateRun(threadId, agent.Id);

        // Poll until the run reaches a terminal status
        do
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            run = agentsClient.Runs.GetRun(threadId, run.Id);
        } while (run.Status == RunStatus.Queued || run.Status == RunStatus.InProgress);
        if (run.Status != RunStatus.Completed)
        {
            throw new InvalidOperationException(
                $"Run failed or was canceled: {run.LastError?.Message}"
            );
        }

        Pageable<PersistentThreadMessage> messages = agentsClient.Messages.GetMessages(
            threadId,
            run.Id,
            order: ListSortOrder.Ascending
        );
        var messageBuilder = new System.Text.StringBuilder();
        foreach (PersistentThreadMessage threadMessage in messages)
        {
            if (threadMessage.Role != MessageRole.Agent)
            {
                continue;
            }
            Console.Write(
                $"{threadMessage.CreatedAt:yyyy-MM-dd HH:mm:ss} - {threadMessage.Role, 10}: "
            );
            foreach (MessageContent contentItem in threadMessage.ContentItems)
            {
                if (contentItem is MessageTextContent textItem)
                {
                    messageBuilder.AppendLine(textItem.Text);
                }
                else if (contentItem is MessageImageFileContent imageFileItem)
                {
                    Console.Write($"<image from ID: {imageFileItem.FileId}");
                }
                Console.WriteLine();
            }
        }

        return Results.Ok(new { result = messageBuilder.ToString() });
    }
);

app.MapDelete(
    "/session/{sessionId}",
    ([FromServices] AgentService agentService, string sessionId) =>
    {
        agentService.RemoveSession(sessionId);
        return Results.Ok(new { message = $"Session {sessionId} removed" });
    }
);

app.Run();

public class AiFoundryOptions
{
    public string EndPointUrl { get; set; } = default!;
    public string AgentId { get; set; } = default!;
}

public record AnalyzeRequest(string SessionId, string? RawData, string? UserPrompt);
