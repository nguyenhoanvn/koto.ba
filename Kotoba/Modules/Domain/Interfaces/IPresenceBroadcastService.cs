using Kotoba.Modules.Domain.DTOs;

namespace Kotoba.Modules.Domain.Interfaces;

/// <summary>
/// Service for broadcasting user presence updates.
/// </summary>
public interface IPresenceBroadcastService
{
    Task<List<PresenceUpdateDto>> GetAllOnlineUsersAsync();
    Task<PresenceUpdateDto> NotifyUserOnlineAsync(string userId);
    Task<PresenceUpdateDto> NotifyUserOfflineAsync(string userId);
}
