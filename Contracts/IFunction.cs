namespace Platform.Auth.Signup.Func.Contracts;

public interface IFunction<TRequest, TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
}
