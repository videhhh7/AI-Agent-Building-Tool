using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace AiAgentBuilder.Services;

public sealed class OpenAiChatService(HttpClient http, IConfiguration configuration)
{
    public async Task<string> AskAsync(string systemPrompt, string userPrompt, CancellationToken cancellationToken)
    {
        var apiKey = configuration["OpenAI:ApiKey"];
        var model = configuration["OpenAI:Model"] ?? "gpt-4.1-mini";
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException("Set OpenAI:ApiKey using user-secrets or an environment variable.");

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Content = JsonContent.Create(new {
            model,
            messages = new[] {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userPrompt }
            }
        });
        using var response = await http.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"AI provider returned {(int)response.StatusCode}: {body}");
        using var json = JsonDocument.Parse(body);
        return json.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString()
            ?? "The model returned an empty response.";
    }
}
