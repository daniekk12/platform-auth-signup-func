namespace Platform.Auth.Signup.Func.Middleware;

public sealed class FunctionOperationCacheMiddleware
{
    private readonly RequestDelegate _next;

    public FunctionOperationCacheMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (ShouldDisableCache(context))
        {
            context.Response.OnStarting(() =>
            {
                var headers = context.Response.Headers;
                headers.CacheControl = "no-store, no-cache, must-revalidate";
                headers.Pragma = "no-cache";
                headers.Expires = "0";
                return Task.CompletedTask;
            });
        }

        await _next(context);
    }

    private static bool ShouldDisableCache(HttpContext context) =>
        HttpMethods.IsPost(context.Request.Method)
        && context.Request.Path.StartsWithSegments("/signup");
}
