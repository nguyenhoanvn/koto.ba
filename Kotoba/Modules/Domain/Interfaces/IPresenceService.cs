namespace Kotoba.Modules.Domain.Interfaces;

/// <summary>
/// Service for managing user online and offline presence.
/// </summary>
public interface IPresenceService
{
    Task SetOnlineAsync(string userId);
    Task SetOfflineAsync(string userId);
    Task<bool> GetUserPresenceAsync(string userId);
    Task<List<string>> GetAllOnlineUsersAsync();
}
