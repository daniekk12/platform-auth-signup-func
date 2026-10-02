using Platform.Auth.Signup.Func.Contracts;
using Platform.Auth.Signup.Func.Models;
using Platform.Auth.Signup.Func.Validation;

namespace Platform.Auth.Signup.Func.Http;

public static class SignupHttpAdapter
{
    public static IEndpointRouteBuilder MapSignupHttpAdapter(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/signup", async (
            SignupRequest? request,
            IFunction<SignupRequest, SignupResponse> signupFunction,
            CancellationToken cancellationToken) =>
        {
            if (request is null)
            {
                return Results.BadRequest(new { message = "Request body is required." });
            }

            try
            {
                var response = await signupFunction.ExecuteAsync(request, cancellationToken);
                return Results.Ok(response);
            }
            catch (FunctionValidationException ex)
            {
                return Results.ValidationProblem(ex.Errors);
            }
        });

        return endpoints;
    }
}
