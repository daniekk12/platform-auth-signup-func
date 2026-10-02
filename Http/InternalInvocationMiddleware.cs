using Platform.Auth.Signup.Func.Configuration;

namespace Platform.Auth.Signup.Func.Http;

public sealed class InternalInvocationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _expectedApiKey;

    public InternalInvocationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _expectedApiKey = configuration[$"{FunctionInvocationOptions.SectionName}:ApiKey"]
            ?? throw new InvalidOperationException(
                $"{FunctionInvocationOptions.SectionName}:ApiKey must be configured.");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (HttpMethods.IsGet(context.Request.Method)
            && context.Request.Path.StartsWithSegments("/health"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(FunctionInvocationOptions.InternalHeaderName, out var provided)
            || !string.Equals(provided.ToString(), _expectedApiKey, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new
            {
                message = "This function is not exposed for direct client access. Call it through the gateway."
            });
            return;
        }

        await _next(context);
    }
}
