namespace Platform.Auth.Signup.Func.Configuration;

public sealed class FunctionInvocationOptions
{
    public const string SectionName = "FunctionInvocation";

    public const string InternalHeaderName = "X-Internal-Api-Key";

    public string ApiKey { get; set; } = string.Empty;
}
