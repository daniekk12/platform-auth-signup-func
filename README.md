# platform-auth-signup-func

Independent **Signup Function** for the Platform Auth learning project. This is not a traditional signup API: business logic lives in `SignupFunction`, which implements `IFunction<SignupRequest, SignupResponse>`. The HTTP endpoint is a **local invocation adapter** only.

## Architecture role

```text
Gateway API  --->  (future Function Host)  --->  Signup Function
```

Today the Gateway calls this component over HTTP. Later, a local Function Host will discover, start, and invoke functions without changing `SignupFunction`.

## Function contract

**Input**

```json
{
  "email": "test@example.com",
  "password": "Password123!"
}
```

**Output**

```json
{
  "message": "Signup function executed"
}
```

C# types: `SignupRequest`, `SignupResponse` in `Contracts/`.

## Run locally

```bash
dotnet run --launch-profile http
```

**Local HTTP adapter:** `POST http://localhost:5001/`

```bash
curl -X POST http://localhost:5001/ -H "Content-Type: application/json" -d "{\"email\":\"test@example.com\",\"password\":\"Password123!\"}"
```

## Build

```bash
dotnet build
```

This repository is standalone: no `.sln`, no project references to other Platform Auth components.
