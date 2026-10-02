using Platform.Auth.Signup.Func.Configuration;
using Platform.Auth.Signup.Func.Security;

namespace Platform.Auth.Signup.Func.Tests;

public sealed class InternalApiKeyValidatorTests
{
    [Fact]
    public void IsValid_returns_true_for_matching_key()
    {
        Assert.True(InternalApiKeyValidator.IsValid("same-key", "same-key"));
    }

    [Fact]
    public void IsValid_returns_false_for_missing_key()
    {
        Assert.False(InternalApiKeyValidator.IsValid("expected-key", null));
    }

    [Fact]
    public void IsValid_returns_false_for_incorrect_key()
    {
        Assert.False(InternalApiKeyValidator.IsValid("expected-key", "wrong-key"));
    }
}
