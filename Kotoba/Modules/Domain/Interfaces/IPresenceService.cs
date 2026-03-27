namespace Kotoba.Modules.Domain.Interfaces;

/// <summary>
/// Service for managing user online and offline presence.
/// </summary>
public interface IPresenceService
{
    Task SetOnlineAsync(string userId, string? connectionId = null);
    Task SetOfflineAsync(string userId, string? connectionId = null);
    Task RegisterConnectionAsync(string userId, string connectionId);
    Task UnregisterConnectionAsync(string userId, string connectionId);
    Task<bool> MarkOfflineIfNoConnectionsAsync(string userId);
    Task<bool> GetUserPresenceAsync(string userId);
    Task<List<string>> GetAllOnlineUsersAsync();
}
