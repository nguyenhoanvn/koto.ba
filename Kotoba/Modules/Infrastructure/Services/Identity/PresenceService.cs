using Kotoba.Modules.Domain.Interfaces;

namespace Kotoba.Modules.Infrastructure.Services.Identity
{
    public class PresenceService : IPresenceService
    {
        public Task<List<string>> GetAllOnlineUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetUserPresenceAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task SetOfflineAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task SetOnlineAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
