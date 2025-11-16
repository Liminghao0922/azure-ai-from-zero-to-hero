using System.Net;
using ManufacturingAi.Web.Components;
using ManufacturingAi.Web.Services;
using Polly;
using Polly.Timeout;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddOutputCache();

var httpClientBuilder = builder
    .Services.AddHttpClient<AnalyzeAiClient>(client =>
    {
        // Use Aspire service discovery when available, fallback to localhost for standalone debugging
        var apiBaseUrl =
            builder.Configuration["Services:ApiService:Https"]
            ?? builder.Configuration["APISERVICE_URL"]
            ?? "https+http://apiservice";

        client.BaseAddress = new Uri(apiBaseUrl);
    })
    .ClearResilienceHandlers()
    // Add service discovery FIRST (required for "apiservice" name resolution)
    .AddServiceDiscovery()
    // Then add custom resilience configuration
    .AddStandardResilienceHandler(options =>
    {
        TimeSpan timeSpan = TimeSpan.FromMinutes(1);

        // Customize retry
        options.Retry.ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
            .Handle<TimeoutRejectedException>()
            .Handle<HttpRequestException>()
            .HandleResult(response => response.StatusCode == HttpStatusCode.InternalServerError);
        options.Retry.MaxRetryAttempts = 5;

        // Customize attempt timeout
        options.AttemptTimeout.Timeout = timeSpan;
        options.CircuitBreaker.SamplingDuration = timeSpan * 2;
        options.TotalRequestTimeout.Timeout = timeSpan * 3;
    });

;
builder.Services.AddScoped<SensorParser>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
