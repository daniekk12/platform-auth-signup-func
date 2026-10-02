namespace Platform.Auth.Signup.Func.Validation;

public sealed class FunctionValidationException : Exception
{
    public FunctionValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base("Request validation failed.")
    {
        Errors = errors;
    }

    public IReadOnlyDictionary<string, string[]> Errors { get; }
}
