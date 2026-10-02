# platform-auth-signup-func

Independent **Signup Function** for the Platform Auth learning project. Business logic lives in `SignupFunction`, which implements `IFunction<SignupRequest, SignupResponse>`. HTTP (`POST /signup`) is a **local invocation adapter** only—not the definition of the function.

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
dotnet run --launch-profile http
```

Listens on **http://localhost:5001** (see `Properties/launchSettings.json`).

## Endpoints

| Method | Path | Description |
|--------|------|-------------|
| POST | `/signup` | Invoke signup function |
| GET | `/health` | Process liveness |

### Request (`POST /signup`)

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

Example:

```bash
curl -X POST http://localhost:5001/signup \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"test@example.com\",\"password\":\"Password123!\"}"
```

## Tests

```bash
dotnet test tests/Platform.Auth.Signup.Func.Tests/Platform.Auth.Signup.Func.Tests.csproj
```

Tests target `SignupFunction` behavior (validation, cancellation), not only HTTP.

Standalone repository: no `.sln`, no project references to other Platform Auth components.
