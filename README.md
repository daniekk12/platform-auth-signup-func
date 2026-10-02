# platform-auth-signup-func

Independent **Signup Function** for Platform Auth. HTTP (`POST /signup`) is an **internal invocation adapter** for the gateway only—not a public client API.

## Client access

**End users and frontends must not call this service directly.** Use the gateway. Direct `POST /signup` without **`X-Internal-Api-Key`** returns **403 Forbidden**.

## Configuration

### Required

| Purpose | Environment variable | Nested key |
|---------|---------------------|------------|
| Internal API key | `FunctionInvocation__ApiKey` | `FunctionInvocation:ApiKey` |

Startup fails if the API key is missing or empty.

### Optional

| Purpose | Default |
|---------|---------|
| Logging levels | Information (see `appsettings.json`) |

### Secrets

- **`FunctionInvocation:ApiKey`** — shared secret with the gateway. Never commit or log.

### Local development

```bash
dotnet user-secrets set "FunctionInvocation:ApiKey" "<your-local-internal-api-key>"
dotnet dev-certs https --trust
dotnet run --launch-profile https
```

Listens on **`https://localhost:5001`**. HTTP is not enabled in the default launch profile.

### Production

```text
FunctionInvocation__ApiKey=<secret>
AllowedHosts__0=<your-public-hostname>
ASPNETCORE_ENVIRONMENT=Production
```

## Endpoints

| Method | Path | Description |
|--------|------|-------------|
| POST | `/signup` | Internal invoke (requires `X-Internal-Api-Key`) |
| GET | `/health` | Liveness (no API key required) |

## Tests

```bash
dotnet test tests/Platform.Auth.Signup.Func.Tests/Platform.Auth.Signup.Func.Tests.csproj
```

## Build & publish

```bash
dotnet build
dotnet publish -c Release
```

Standalone repository: no project references to other Platform Auth components.
