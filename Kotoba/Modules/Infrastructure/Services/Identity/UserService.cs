using Kotoba.Modules.Domain.DTOs;
using Kotoba.Modules.Domain.Interfaces;

namespace Kotoba.Modules.Infrastructure.Services.Identity
{
    public class UserService : IUserService
    {
        public Task<UserProfile?> GetUserProfileAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LoginAsync(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterAsync(RegisterRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserProfileAsync(string userId, UpdateProfileRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
