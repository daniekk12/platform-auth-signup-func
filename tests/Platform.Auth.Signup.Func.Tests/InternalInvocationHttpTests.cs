using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Platform.Auth.Signup.Func.Configuration;

namespace Platform.Auth.Signup.Func.Tests;

public sealed class InternalInvocationHttpTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string TestApiKey = "test-internal-key";

    [Fact]
    public async Task Signup_without_internal_key_returns_forbidden()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/signup",
            new { email = "user@example.com", password = "Password123!" });

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Signup_with_valid_internal_key_succeeds()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add(FunctionInvocationOptions.InternalHeaderName, TestApiKey);

        var response = await client.PostAsJsonAsync(
            "/signup",
            new { email = "user@example.com", password = "Password123!" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Health_does_not_require_internal_key()
    {
        await using var factory = CreateFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private static WebApplicationFactory<Program> CreateFactory() =>
        new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            builder.UseSetting($"{FunctionInvocationOptions.SectionName}:ApiKey", TestApiKey));
}
