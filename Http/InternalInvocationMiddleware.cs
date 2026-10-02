using Microsoft.Extensions.Options;
using Platform.Auth.Signup.Func.Configuration;
using Platform.Auth.Signup.Func.Security;

namespace Platform.Auth.Signup.Func.Http;

public sealed class InternalInvocationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly FunctionInvocationOptions _options;

    public InternalInvocationMiddleware(RequestDelegate next, IOptions<FunctionInvocationOptions> options)
    {
        _next = next;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (HttpMethods.IsGet(context.Request.Method)
            && context.Request.Path.StartsWithSegments("/health"))
        {
            await _next(context);
            return;
        }

        context.Request.Headers.TryGetValue(FunctionInvocationOptions.InternalHeaderName, out var provided);

        if (!InternalApiKeyValidator.IsValid(_options.ApiKey, provided.ToString()))
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
