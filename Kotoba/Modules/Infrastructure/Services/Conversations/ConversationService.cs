using Kotoba.Modules.Domain.DTOs;
using Kotoba.Modules.Domain.Interfaces;

namespace Kotoba.Modules.Infrastructure.Services.Conversations
{
    public class ConversationService : IConversationService
    {
        public Task<ConversationDto?> CreateDirectConversationAsync(string userAId, string userBId)
        {
            throw new NotImplementedException();
        }

        public Task<ConversationDto?> CreateGroupConversationAsync(CreateGroupRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ConversationDto?> GetConversationDetailAsync(Guid conversationId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ConversationDto>> GetUserConversationsAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
