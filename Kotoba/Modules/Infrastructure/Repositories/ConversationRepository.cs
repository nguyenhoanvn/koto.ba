using Kotoba.Modules.Domain.Entities;
using Kotoba.Modules.Domain.Interfaces;
using Kotoba.Modules.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kotoba.Modules.Infrastructure.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly KotobaDbContext _context;

        public ConversationRepository(KotobaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Conversation>> GetAllAsync()
        {
            return await _context.Conversations
                .ToListAsync();
        }

        public async Task<Conversation?> GetAsync(Guid conversationId)
        {
            return await _context.Conversations
                .FirstOrDefaultAsync(c => c.Id == conversationId);
        }
    }
}
