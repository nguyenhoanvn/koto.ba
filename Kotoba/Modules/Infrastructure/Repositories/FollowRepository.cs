using Kotoba.Modules.Domain.Entities;
using Kotoba.Modules.Domain.Interfaces;
using Kotoba.Modules.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Kotoba.Modules.Infrastructure.Repositories

{
    public class FollowRepository : IFollowRepository
    {
        private readonly IDbContextFactory<KotobaDbContext> _dbFactory;

        public FollowRepository(IDbContextFactory<KotobaDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<bool> FollowAsync(string followerId, string followingId)
        {
            using (var context = await _dbFactory.CreateDbContextAsync())
            {
                var existing = await context.Follows
                    .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

                if (existing != null)
                    return false; 

                var follow = new Follow
                {
                    Id = Guid.NewGuid(),
                    FollowerId = followerId,
                    FollowingId = followingId,
                    CreatedAt = DateTime.UtcNow
                };

                context.Follows.Add(follow);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UnfollowAsync(string followerId, string followingId)
        {
            using (var context = await _dbFactory.CreateDbContextAsync())
            {
                var follow = await context.Follows
                    .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

                if (follow == null)
                    return false;

                context.Follows.Remove(follow);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> IsFollowingAsync(string followerId, string followingId)
        {
            using (var context = await _dbFactory.CreateDbContextAsync())
            {
                return await context.Follows
                    .AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
            }
        }

        public async Task<List<string>> GetFollowingIdsAsync(string userId)
        {
            using (var context = await _dbFactory.CreateDbContextAsync())
            {
                return await context.Follows
                    .Where(f => f.FollowerId == userId)
                    .Select(f => f.FollowingId)
                    .ToListAsync();
            }
        }
    }
}
