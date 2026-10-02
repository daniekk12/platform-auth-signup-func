using System.ComponentModel.DataAnnotations;

namespace Platform.Auth.Signup.Func.Validation;

internal static class RequestValidation
{
    internal static IReadOnlyDictionary<string, string[]> Validate(object instance)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(instance);

        if (Validator.TryValidateObject(instance, context, results, validateAllProperties: true))
        {
            return new Dictionary<string, string[]>();
        }

        return results
            .GroupBy(
                result => result.MemberNames.FirstOrDefault() ?? string.Empty,
                result => result.ErrorMessage ?? "Validation failed.")
            .ToDictionary(
                group => group.Key,
                group => group.ToArray());
    }
}
