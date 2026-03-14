using Kotoba.Modules.Domain.Entities;

namespace Kotoba.Modules.Domain.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetAllAsync();
        Task AddAsync(Message message);
        Task<Message?> GetAsync(Guid messageId);
        Task<IEnumerable<Message>> GetAllByConversationIdAsync(Guid conversationId);
        Task<IEnumerable<Message>> GetMessagesPageAsync(Guid conversationId, int page, int pageSize);
    }
}
