using Platform.Auth.Signup.Func.Abstractions;
using Platform.Auth.Signup.Func.Contracts;

namespace Platform.Auth.Signup.Func.Functions;

public sealed class SignupFunction : IFunction<SignupRequest, SignupResponse>
{
    public Task<SignupResponse> ExecuteAsync(
        SignupRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _ = request;

        return Task.FromResult(new SignupResponse("Signup function executed"));
    }
}
