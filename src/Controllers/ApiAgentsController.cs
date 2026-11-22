using AiAgentBuilder.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiAgentBuilder.Controllers;

[ApiController]
[Route("api/agents")]
public sealed class ApiAgentsController(OpenAiChatService ai) : ControllerBase
{
    public sealed record RunAgentRequest(string Instructions, string Prompt);

    [HttpPost("run")]
    public async Task<IActionResult> Run([FromBody] RunAgentRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Prompt) || request.Prompt.Length > 12000)
            return BadRequest(new { error = "Prompt is required and must be 12,000 characters or fewer." });
        var answer = await ai.AskAsync(
            string.IsNullOrWhiteSpace(request.Instructions) ? "You are a helpful assistant." : request.Instructions,
            request.Prompt, cancellationToken);
        return Ok(new { answer });
    }
}
