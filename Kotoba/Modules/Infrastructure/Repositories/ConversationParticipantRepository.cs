using Kotoba.Modules.Domain.DTOs;
using Kotoba.Modules.Domain.Entities;
using Kotoba.Modules.Domain.Enums;
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

        public IQueryable<Guid> GetAllConversationIdsForUserAsync(string userId)
        {
            return _context.ConversationParticipants
            .Where(p => p.UserId == userId && p.IsActive)
            .Select(p => p.ConversationId);
        }

        public async Task AddAsync(ConversationParticipant participant)
        {
            await _context.ConversationParticipants.AddAsync(participant);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ConversationParticipant>> GetAllConversationByGroupNameForUserAsync(string userId, string groupName)
        {
            return await _context.ConversationParticipants
            .Include(p => p.Conversation)
            .Where(p => p.UserId == userId
                        && p.IsActive
                        && p.Conversation.Type == ConversationType.Group
                        && p.Conversation.GroupName != null
                        && p.Conversation.GroupName.Contains(groupName))
            .ToListAsync();
        }

        public async Task<List<ConversationParticipant>> GetAllConversationByUserAsync(string userId)
        {
            return await _context.ConversationParticipants
            .Include(p => p.Conversation)
            .Where(p => p.UserId == userId
                        && p.IsActive
                        && p.Conversation.Type == ConversationType.Direct)
            .ToListAsync();

        }

        public async Task<List<UserProfile>> GetOtherUsersInConversationAsync(string conversationId, string userId)
        {
            return await _context.ConversationParticipants
                .Where(p => p.ConversationId.ToString() == conversationId
                         && p.UserId != userId
                         && p.IsActive)
                .Select(p => new UserProfile
                {
                    UserId = p.User.Id,
                    DisplayName = p.User.DisplayName,
                    AvatarUrl = p.User.AvatarUrl,
                    IsOnline = p.User.IsOnline,
                    LastSeenAt = p.User.LastSeenAt
                })
                .ToListAsync();
        }
    }
}
