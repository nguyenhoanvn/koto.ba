using Kotoba.Modules.Domain.Entities;
using Kotoba.Modules.Domain.Interfaces;
using Kotoba.Modules.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kotoba.Modules.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly KotobaDbContext _context;

        public MessageRepository(KotobaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            return await _context.Messages.ToListAsync();
        }

        public async Task<Message?> GetAsync(Guid messageId)
        {
            return await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public async Task<IEnumerable<Message>> GetAllByConversationIdAsync(Guid conversationId)
        {
            return await _context.Messages
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
        }

        public async Task<IEnumerable<Message>> GetMessagesPageAsync(Guid conversationId, int page, int pageSize)
        {
            pageSize = Math.Clamp(pageSize, 1, 100);
            page = Math.Max(page, 1);

            var messages = await _context.Messages
                .Where(m => m.ConversationId == conversationId && !m.IsDeleted)
                .OrderByDescending(m => m.CreatedAt)
            .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(m => m.Reactions)
                .Include(m => m.Attachments)
                .AsNoTracking()
                .ToListAsync();

            return messages
                .OrderBy(m => m.CreatedAt)
                .ToList();
        }
    }
}
