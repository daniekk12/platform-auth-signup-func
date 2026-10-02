namespace Platform.Auth.Signup.Func.Abstractions;

public interface IFunction<TRequest, TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
}
