namespace Platform.Auth.Signup.Func.Security;

public static class InternalApiKeyValidator
{
    public static bool IsValid(string expectedApiKey, string? providedApiKey)
    {
        var expectedHash = System.Security.Cryptography.SHA256.HashData(
            System.Text.Encoding.UTF8.GetBytes(expectedApiKey ?? string.Empty));
        var providedHash = System.Security.Cryptography.SHA256.HashData(
            System.Text.Encoding.UTF8.GetBytes(providedApiKey ?? string.Empty));

        return System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(expectedHash, providedHash);
    }
}
