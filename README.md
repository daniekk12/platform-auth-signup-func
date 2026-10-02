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

## Deploy to Render (free tier — learning / demo)

Free Web Services **sleep**, **cold start**, and have **limited resources**. This is a demo layout, not a production SLA.

**Deploy this service first** in the Platform Auth sequence (before login, then gateway).

### Render Web Service

| Setting | Value |
|---------|--------|
| Environment | Docker |
| Dockerfile | `./Dockerfile` |
| Health check | `/health` |
| Plan | Free |

### Environment variables (Render dashboard)

| Key | Sensitive | Notes |
|-----|-----------|--------|
| `FunctionInvocation__ApiKey` | Yes | Same secret as gateway and login |
| `AllowedHosts__0` | No | `<your-signup-service>.onrender.com` |
| `ASPNETCORE_ENVIRONMENT` | No | `Production` |

On free tier the service URL is **public**. **`X-Internal-Api-Key`** is still required on `POST /signup`; that is not network isolation.

Render injects **`PORT`**; the app listens on `0.0.0.0` when `PORT` is set.

### Docker (local)

```bash
docker build -t platform-auth-signup .
docker run --rm -p 8080:8080 -e PORT=8080 \
  -e FunctionInvocation__ApiKey="<your-local-internal-api-key>" \
  -e ASPNETCORE_ENVIRONMENT=Production \
  platform-auth-signup
curl http://localhost:8080/health
```

### GitHub → Render

Connect this repo in Render (auto-deploy from `main`) or set `RENDER_DEPLOY_HOOK_URL` for `.github/workflows/render-deploy.yml`. CI: `.github/workflows/ci.yml`.

### Verify

```bash
curl -fsS "https://<your-signup-host>/health"
```
