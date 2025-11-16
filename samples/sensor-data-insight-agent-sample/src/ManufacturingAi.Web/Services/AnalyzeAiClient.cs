namespace ManufacturingAi.Web.Services;

public class AnalyzeAiClient(HttpClient httpClient)
{
    public async Task<string> AnalyzeAsync(string sessionId, string? rawData, string? userPrompt = null)
    {
        var response = await httpClient.PostAsJsonAsync("/analyze", new { SessionId = sessionId, RawData = rawData, UserPrompt = userPrompt });
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<AnalyzeResponse>();
        return result?.Result ?? "応答がありません";
    }

    public async Task ClearSessionAsync(string sessionId)
    {
        var response = await httpClient.DeleteAsync($"/session/{sessionId}");
        response.EnsureSuccessStatusCode();
    }

    private record AnalyzeResponse(string Result);
}
