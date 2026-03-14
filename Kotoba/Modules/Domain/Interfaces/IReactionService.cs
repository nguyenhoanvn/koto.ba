using Kotoba.Modules.Domain.DTOs;
using Kotoba.Modules.Domain.Enums;

namespace Kotoba.Modules.Domain.Interfaces;

/// <summary>
/// Service for managing message reactions.
/// </summary>
public interface IReactionService
{
    Task<ReactionDto?> AddOrUpdateReactionAsync(string userId, Guid messageId, ReactionType reactionType);
    Task<bool> RemoveReactionAsync(string userId, Guid messageId);
    Task<List<ReactionDto>> GetReactionsAsync(Guid messageId);
}
