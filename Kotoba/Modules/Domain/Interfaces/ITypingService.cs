namespace Kotoba.Modules.Domain.Interfaces;

/// <summary>
/// Service for managing typing indicators.
/// </summary>
public interface ITypingService
{
    Task SetTypingAsync(string userId, Guid conversationId, bool isTyping);
}
