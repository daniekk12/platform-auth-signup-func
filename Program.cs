using Platform.Auth.Signup.Func.Abstractions;
using Platform.Auth.Signup.Func.Contracts;
using Platform.Auth.Signup.Func.Functions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<SignupFunction>();
builder.Services.AddSingleton<IFunction<SignupRequest, SignupResponse>>(sp =>
    sp.GetRequiredService<SignupFunction>());

var app = builder.Build();

app.MapPost("/signup", async (
    SignupRequest request,
    IFunction<SignupRequest, SignupResponse> signupFunction,
    CancellationToken cancellationToken) =>
{
    var response = await signupFunction.ExecuteAsync(request, cancellationToken);
    return Results.Ok(response);
});

app.Run();
