using Kotoba.Modules.Domain.DTOs;
using Kotoba.Modules.Domain.Enums;
using Kotoba.Modules.Domain.Interfaces;

namespace Kotoba.Modules.Infrastructure.Services.Reactions
{
    public class ReactionService : IReactionService
    {
        public Task<ReactionDto?> AddOrUpdateReactionAsync(string userId, Guid messageId, ReactionType reactionType)
        {
            throw new NotImplementedException();
        }

        public Task<List<ReactionDto>> GetReactionsAsync(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveReactionAsync(string userId, Guid messageId)
        {
            throw new NotImplementedException();
        }
    }
}
