# AI Agent Building Tool

ASP.NET Core MVC with a real OpenAI API integration.

## Requirements
.NET 8 SDK and an OpenAI API key with API billing enabled.

## Credentials (never commit keys)
Run in this repository:
```bash
dotnet user-secrets init
dotnet user-secrets set "OpenAI:ApiKey" "YOUR_API_KEY"
dotnet user-secrets set "OpenAI:Model" "gpt-4.1-mini"
```
Alternatively set `OpenAI__ApiKey` and `OpenAI__Model` environment variables. In cloud hosting, store the key in the platform secret store or AWS Secrets Manager and inject it at runtime.

## Run
```bash
dotnet restore
dotnet run
```

## API
`POST /api/agents/run`
```json
{"instructions":"You are a concise .NET mentor.","prompt":"Explain dependency injection in C# with an example."}
```
The server sends the request to OpenAI Chat Completions; the key is never sent to browser code. API use may incur charges. Do not send personal, passenger, employer-confidential or other sensitive data.

## Cloud
Deploy to AWS App Runner or ECS. Configure `OpenAI__ApiKey` using Secrets Manager integration / task secret injection, enable HTTPS, authentication, per-user rate limits and request logging with prompt redaction. Restrict outbound network access as appropriate.