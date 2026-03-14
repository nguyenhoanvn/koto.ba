using Kotoba.Modules.Domain.DTOs;

namespace Kotoba.Modules.Domain.Interfaces;

/// <summary>
/// Service for broadcasting realtime chat events.
/// </summary>
public interface IRealtimeChatService
{
    Task BroadcastMessageAsync(MessageDto message);
    Task BroadcastTypingAsync(TypingStatusDto typingStatus);
}
