using Azure.AI.Agents.Persistent;
using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace ManufacturingAi.ApiService.Services;

public class AgentService
{
    private readonly Lazy<PersistentAgentsClient> _agentsClient;
    private readonly Lazy<PersistentAgent> _agent;
    private readonly IMemoryCache _cache;
    private readonly ConcurrentDictionary<string, SessionContext> _sessions = new();

    public AgentService(IOptions<AiFoundryOptions> options, IMemoryCache cache)
    {
        _cache = cache;
        _agentsClient = new Lazy<PersistentAgentsClient>(() =>
        {
            var endpoint = new Uri(options.Value.EndPointUrl);
            var projectClient = new AIProjectClient(endpoint, new DefaultAzureCredential());
            return projectClient.GetPersistentAgentsClient();
        });

        _agent = new Lazy<PersistentAgent>(() =>
        {
            return _agentsClient.Value.Administration.GetAgent(options.Value.AgentId);
        });
    }

    public PersistentAgentsClient AgentsClient => _agentsClient.Value;
    public PersistentAgent Agent => _agent.Value;

    /// <summary>
    /// Get or create a thread for a session. If rawData is provided and different from cached data,
    /// a new thread will be created.
    /// </summary>
    public SessionContext GetOrCreateSession(string sessionId, string? rawData = null)
    {
        var session = _sessions.GetOrAdd(sessionId, _ => new SessionContext());

        // If rawData is provided and different from existing, create new thread
        if (!string.IsNullOrWhiteSpace(rawData) && session.RawDataHash != ComputeHash(rawData))
        {
            PersistentAgentThread newThread = AgentsClient.Threads.CreateThread();
            session.ThreadId = newThread.Id;
            session.RawDataHash = ComputeHash(rawData);
            session.HasDataContext = false; // Mark that we need to send data in next message
            
            Console.WriteLine($"Created new thread for session {sessionId}: {newThread.Id}");
        }
        // If no thread exists yet, create one
        else if (string.IsNullOrWhiteSpace(session.ThreadId))
        {
            PersistentAgentThread newThread = AgentsClient.Threads.CreateThread();
            session.ThreadId = newThread.Id;
            
            Console.WriteLine($"Created initial thread for session {sessionId}: {newThread.Id}");
        }

        return session;
    }

    /// <summary>
    /// Remove a session and its cached thread
    /// </summary>
    public void RemoveSession(string sessionId)
    {
        _sessions.TryRemove(sessionId, out _);
        Console.WriteLine($"Removed session {sessionId}");
    }

    private static string ComputeHash(string input)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}

public class SessionContext
{
    public string ThreadId { get; set; } = string.Empty;
    public string RawDataHash { get; set; } = string.Empty;
    public bool HasDataContext { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;
}
