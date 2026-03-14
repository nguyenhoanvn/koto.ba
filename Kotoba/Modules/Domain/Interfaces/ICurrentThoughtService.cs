namespace Kotoba.Modules.Domain.Interfaces;

/// <summary>
/// Service for managing the current thought for a user.
/// </summary>
public interface ICurrentThoughtService
{
    Task<bool> SetThoughtAsync(string userId, string content);
    Task<string?> GetThoughtAsync(string userId);
}
