using Kotoba.Modules.Domain.Entities;
using Kotoba.Modules.Domain.Enums;
using Kotoba.Modules.Domain.Interfaces;
using Kotoba.Modules.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kotoba.Modules.Infrastructure.Services.Social
{
    public class CurrentThoughtService : ICurrentThoughtService
    {
        private readonly IDbContextFactory<KotobaDbContext> _dbFactory;
        private static readonly TimeSpan ThoughtLifetime = TimeSpan.FromHours(24);

        public CurrentThoughtService(IDbContextFactory<KotobaDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<string?> GetThoughtAsync(string userId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var thought = await db.CurrentThoughts
                .FirstOrDefaultAsync(ct => ct.UserId == userId);
            if (thought == null) return null;
            if (thought.ExpiresAt <= now)
            {
                db.CurrentThoughts.Remove(thought);
                await db.SaveChangesAsync();
                return null;
            }
            return thought.Content;
        }

        public async Task<bool> SetThoughtAsync(string userId, string content)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(content))
                return false;
            using var db = await _dbFactory.CreateDbContextAsync();

            var user = await db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.AccountStatus != AccountStatus.Active)
                return false;

            var existing = await db.CurrentThoughts
                .FirstOrDefaultAsync(ct => ct.UserId == userId);
            if (existing is null)
            {
                db.CurrentThoughts.Add(new CurrentThought
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Content = content,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.Add(ThoughtLifetime)
                });
            }
            else
            {
                existing.Content = content;
                existing.CreatedAt = DateTime.UtcNow;
                existing.ExpiresAt = DateTime.UtcNow.Add(ThoughtLifetime);
            }
            await db.SaveChangesAsync();
            return true;
        }
    }
}
