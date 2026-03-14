using Kotoba.Modules.Domain.Entities;
using Kotoba.Modules.Domain.Interfaces;
using Kotoba.Modules.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kotoba.Modules.Infrastructure.Repositories
{
    public class ConversationParticipantRepository : IConversationParticipantRepository
    {
        private readonly KotobaDbContext _context;

        public ConversationParticipantRepository(KotobaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ConversationParticipant>> GetAllAsync()
        {
            return await _context.ConversationParticipants.ToListAsync();
        }

        public async Task<bool> IsParticipant(Guid conversationId, string senderId)
        {
            return await _context.ConversationParticipants
            .AnyAsync(p => p.ConversationId == conversationId
                        && p.UserId == senderId
                        && p.IsActive);
        }
    }
}
