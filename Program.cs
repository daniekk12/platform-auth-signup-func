using Platform.Auth.Signup.Func.Contracts;
using Platform.Auth.Signup.Func.Functions;
using Platform.Auth.Signup.Func.Http;
using Platform.Auth.Signup.Func.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<SignupFunction>();
builder.Services.AddSingleton<IFunction<SignupRequest, SignupResponse>>(sp =>
    sp.GetRequiredService<SignupFunction>());
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapSignupHttpAdapter();
app.MapHealthChecks("/health");

app.Run();

public partial class Program;
