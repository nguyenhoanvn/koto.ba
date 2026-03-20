using Kotoba.Modules.Domain.DTOs;
using Kotoba.Modules.Infrastructure.Data;
using System.Buffers;

namespace Kotoba.Modules.Infrastructure.Repositories
{
    public class UserProfileRepository
    {
        private readonly KotobaDbContext _context;

        public UserProfileRepository(KotobaDbContext context)
        {
            _context = context;
        }

        public IQueryable<UserProfile> GetUsersByDisplayNameAsync(string searchValue)
        {
            return _context.Users
                .Where(u => u.DisplayName.Contains(searchValue))
                .Select(u => new UserProfile { UserId = u.Id, DisplayName = u.DisplayName });
        }
    }
}
