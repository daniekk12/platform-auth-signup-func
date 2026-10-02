using Platform.Auth.Signup.Func.Contracts;
using Platform.Auth.Signup.Func.Extensions;
using Platform.Auth.Signup.Func.Functions;
using Platform.Auth.Signup.Func.Hosting;
using Platform.Auth.Signup.Func.Http;
using Platform.Auth.Signup.Func.Middleware;
using Platform.Auth.Signup.Func.Models;

ContainerPortBinding.ApplyIfConfigured();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignupConfiguration(builder.Configuration);
builder.Services.AddSingleton<SignupFunction>();
builder.Services.AddSingleton<IFunction<SignupRequest, SignupResponse>>(sp =>
    sp.GetRequiredService<SignupFunction>());
builder.Services.AddHealthChecks();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseMiddleware<FunctionOperationCacheMiddleware>();
app.UseMiddleware<InternalInvocationMiddleware>();

app.MapSignupHttpAdapter();
app.MapHealthChecks("/health");

app.Run();

public partial class Program;
