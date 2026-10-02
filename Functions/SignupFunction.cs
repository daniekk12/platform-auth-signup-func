using Platform.Auth.Signup.Func.Contracts;
using Platform.Auth.Signup.Func.Models;
using Platform.Auth.Signup.Func.Validation;

namespace Platform.Auth.Signup.Func.Functions;

public sealed class SignupFunction : IFunction<SignupRequest, SignupResponse>
{
    private readonly ILogger<SignupFunction> _logger;

    public SignupFunction(ILogger<SignupFunction> logger)
    {
        _logger = logger;
    }

    public Task<SignupResponse> ExecuteAsync(
        SignupRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation("Signup function invoked");

        ArgumentNullException.ThrowIfNull(request);

        var errors = RequestValidation.Validate(request);
        if (errors.Count > 0)
        {
            throw new FunctionValidationException(errors);
        }

        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(
            new SignupResponse("Signup function executed", request.Email.Trim()));
    }
}
