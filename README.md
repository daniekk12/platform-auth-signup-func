# platform-auth-signup-func

Independent **Signup Function** for the Platform Auth learning project. Business logic lives in `SignupFunction`, which implements `IFunction<SignupRequest, SignupResponse>`. HTTP (`POST /signup`) is a **local invocation adapter** for the gateway (or a future Function Host)—**not** a public client API.

## Client access

**End users and frontends must not call this service directly.** Use the gateway at `https://localhost:5000/auth/signup`. Direct `POST /signup` without the internal invocation header returns **403 Forbidden**.

## What it does

- Validates signup input (email format, required fields, minimum password length).
- Executes signup function logic without database, persistence, or password hashing.
- Returns a non-sensitive success payload demonstrating the function ran.

## Architecture

```text
HTTP Request (POST /signup)
        |
        v
  HTTP Adapter (Http/SignupHttpAdapter)
        |
        v
  SignupFunction (Functions/SignupFunction)
        |
        v
  SignupResponse
```

A future Function Host will invoke `SignupFunction` directly without changing business logic.

## Install

```bash
dotnet restore
dotnet build
```

## Run locally

```bash
dotnet dev-certs https --trust
dotnet run --launch-profile https
```

Listens on **https://localhost:5001** (see `Properties/launchSettings.json`). HTTP is not enabled in the default launch profile.

## Endpoints

| Method | Path | Description |
|--------|------|-------------|
| POST | `/signup` | Gateway-internal invoke (requires `X-Platform-Auth-Internal-Key`) |
| GET | `/health` | Process liveness |

Configure `FunctionInvocation:ApiKey` (same shared secret as the gateway). Development default is in `appsettings.Development.json`.

### Request (`POST /signup`, gateway only)

```json
{
  "email": "test@example.com",
  "password": "Password123!"
}
```

### Success response (`200 OK`)

```json
{
  "message": "Signup function executed",
  "email": "test@example.com"
}
```

### Validation error (`400 Bad Request`)

Validation problem details; passwords and secrets are never returned.

## Tests

```bash
dotnet test tests/Platform.Auth.Signup.Func.Tests/Platform.Auth.Signup.Func.Tests.csproj
```

Tests target `SignupFunction` behavior (validation, cancellation), not only HTTP.

Standalone repository: no `.sln`, no project references to other Platform Auth components.
