using Platform.Auth.Signup.Func.Configuration;

namespace Platform.Auth.Signup.Func.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddSignupConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<FunctionInvocationOptions>()
            .Bind(configuration.GetSection(FunctionInvocationOptions.SectionName))
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.ApiKey),
                $"{FunctionInvocationOptions.SectionName}.{nameof(FunctionInvocationOptions.ApiKey)} is required.")
            .ValidateOnStart();

        return services;
    }
}
