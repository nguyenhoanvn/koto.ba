namespace Kotoba.Modules.Domain.DTOs;

public class AccountOperationResult
{
    public bool Succeeded { get; init; }
    public List<string> Errors { get; init; } = new();

    public static AccountOperationResult Success()
    {
        return new AccountOperationResult { Succeeded = true };
    }

    public static AccountOperationResult Failure(IEnumerable<string> errors)
    {
        return new AccountOperationResult
        {
            Succeeded = false,
            Errors = errors
                .Where(error => !string.IsNullOrWhiteSpace(error))
                .Select(error => error.Trim())
                .Distinct(StringComparer.Ordinal)
                .ToList()
        };
    }
}
