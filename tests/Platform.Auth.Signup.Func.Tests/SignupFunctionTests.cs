using Microsoft.Extensions.Logging.Abstractions;
using Platform.Auth.Signup.Func.Functions;
using Platform.Auth.Signup.Func.Models;
using Platform.Auth.Signup.Func.Validation;

namespace Platform.Auth.Signup.Func.Tests;

public sealed class SignupFunctionTests
{
    private readonly SignupFunction _function = new(NullLogger<SignupFunction>.Instance);

    [Fact]
    public async Task ExecuteAsync_valid_request_returns_success_response()
    {
        var request = new SignupRequest
        {
            Email = "test@example.com",
            Password = "Password123!"
        };

        var response = await _function.ExecuteAsync(request, CancellationToken.None);

        Assert.Equal("Signup function executed", response.Message);
        Assert.Equal("test@example.com", response.Email);
    }

    [Fact]
    public async Task ExecuteAsync_missing_email_throws_validation_exception()
    {
        var request = new SignupRequest { Email = string.Empty, Password = "Password123!" };

        var ex = await Assert.ThrowsAsync<FunctionValidationException>(
            () => _function.ExecuteAsync(request, CancellationToken.None));

        Assert.Contains(ex.Errors, pair => pair.Key == nameof(SignupRequest.Email));
    }

    [Fact]
    public async Task ExecuteAsync_invalid_email_throws_validation_exception()
    {
        var request = new SignupRequest { Email = "not-an-email", Password = "Password123!" };

        var ex = await Assert.ThrowsAsync<FunctionValidationException>(
            () => _function.ExecuteAsync(request, CancellationToken.None));

        Assert.Contains(ex.Errors, pair => pair.Key == nameof(SignupRequest.Email));
    }

    [Fact]
    public async Task ExecuteAsync_missing_password_throws_validation_exception()
    {
        var request = new SignupRequest { Email = "test@example.com", Password = string.Empty };

        var ex = await Assert.ThrowsAsync<FunctionValidationException>(
            () => _function.ExecuteAsync(request, CancellationToken.None));

        Assert.Contains(ex.Errors, pair => pair.Key == nameof(SignupRequest.Password));
    }

    [Fact]
    public async Task ExecuteAsync_password_too_short_throws_validation_exception()
    {
        var request = new SignupRequest { Email = "test@example.com", Password = "short" };

        var ex = await Assert.ThrowsAsync<FunctionValidationException>(
            () => _function.ExecuteAsync(request, CancellationToken.None));

        Assert.Contains(ex.Errors, pair => pair.Key == nameof(SignupRequest.Password));
    }

    [Fact]
    public async Task ExecuteAsync_cancelled_token_throws_operation_canceled()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var request = new SignupRequest
        {
            Email = "test@example.com",
            Password = "Password123!"
        };

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => _function.ExecuteAsync(request, cts.Token));
    }
}
