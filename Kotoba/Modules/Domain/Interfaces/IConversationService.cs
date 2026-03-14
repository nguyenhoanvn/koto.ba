using Kotoba.Modules.Domain.DTOs;

namespace Kotoba.Modules.Domain.Interfaces;

/// <summary>
/// Service for managing direct and group conversations.
/// </summary>
public interface IConversationService
{
    Task<ConversationDto?> CreateDirectConversationAsync(string userAId, string userBId);
    Task<ConversationDto?> CreateGroupConversationAsync(CreateGroupRequest request);
    Task<List<ConversationDto>> GetUserConversationsAsync(string userId);
    Task<ConversationDto?> GetConversationDetailAsync(Guid conversationId);
}
