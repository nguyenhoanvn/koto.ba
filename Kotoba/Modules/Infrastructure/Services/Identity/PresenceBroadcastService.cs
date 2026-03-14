using Kotoba.Modules.Domain.DTOs;
using Kotoba.Modules.Domain.Interfaces;

namespace Kotoba.Modules.Infrastructure.Services.Identity
{
    public class PresenceBroadcastService : IPresenceBroadcastService
    {
        public Task<List<PresenceUpdateDto>> GetAllOnlineUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PresenceUpdateDto> NotifyUserOfflineAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<PresenceUpdateDto> NotifyUserOnlineAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
