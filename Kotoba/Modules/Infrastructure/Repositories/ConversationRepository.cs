using Kotoba.Modules.Domain.DTOs;
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

        public async Task<ConversationDto?> GetConversationByIdAsync(Guid conversationId)
        {
            return await _context.Conversations
                .Where(c => c.Id == conversationId && c.Type == Domain.Enums.ConversationType.Direct)
                .Select(c => new ConversationDto
                {
                    ConversationId = c.Id,
                    Type = c.Type,
                    GroupName = c.GroupName,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Conversation conversation)
        {
            await _context.Conversations.AddAsync(conversation);
            await _context.SaveChangesAsync();
        }

        public Task<Conversation?> GetAsync(Guid conversationId)
        {
            throw new NotImplementedException();
        }

        public async Task<ConversationDto?> GetConversationDetailByIdAsync(string conversationId)
        {
            return await _context.Conversations
                .Where(c => conversationId != null && c.Id.ToString() == conversationId)
                .Select(c => new ConversationDto
                {
                    ConversationId = c.Id,
                    Type = c.Type,                    
                    GroupName = c.GroupName,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .FirstOrDefaultAsync();
        }
    }
}
