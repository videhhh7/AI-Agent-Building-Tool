# AI Agent Building Tool

ASP.NET Core MVC starter application for defining and running named agents through configured HTTPS endpoints.

## Features
- MVC interface for agent instructions and prompts.
- Server-side endpoint registry; the browser cannot submit an arbitrary target URL.
- Typed HttpClient with timeout and cancellation support.
- JSON request/response handling.
- HTTPS-only endpoint validation and anti-forgery validation.

## Run locally
Requires the .NET 8 SDK.

1. Copy `appsettings.example.json` to `appsettings.Development.json`.
2. Set `AgentEndpoints:DemoAgent` to an HTTPS endpoint you own or are authorized to call.
3. From this directory run:

```bash
dotnet restore
dotnet build
dotnet run
```

Open the local URL printed by ASP.NET Core.

## Production hardening
Add authentication/authorization, per-user quotas, persistence, audit logging, response-size limits, outbound host allow-list enforcement, rate limiting and automated tests before public deployment. Do not store API keys in appsettings or commit secrets.

## Status
Interview portfolio starter implementation. Review and test against your own endpoint before demonstrating it live.
