using Kotoba.Modules.Domain.Interfaces;
using Kotoba.Modules.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace Kotoba.Modules.Infrastructure.Services.Identity
{
    public class PresenceService : IPresenceService
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> _userConnections = new();
        private readonly IDbContextFactory<KotobaDbContext> _dbFactory;

        public PresenceService(IDbContextFactory<KotobaDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public Task<List<string>> GetAllOnlineUsersAsync()
        {
            var onlineUsers = _userConnections
                .Where(kvp => !kvp.Value.IsEmpty)
                .Select(kvp => kvp.Key)
                .ToList();

            return Task.FromResult(onlineUsers);
        }

        public Task<bool> GetUserPresenceAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(_userConnections.TryGetValue(userId, out var connections) && !connections.IsEmpty);
        }

        public async Task RegisterConnectionAsync(string userId, string connectionId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(connectionId))
            {
                return;
            }

            var connections = _userConnections.GetOrAdd(userId, _ => new ConcurrentDictionary<string, byte>());
            var wasOffline = connections.IsEmpty;
            connections[connectionId] = 0;

            if (wasOffline)
            {
                await SetUserPresenceInDatabaseAsync(userId, isOnline: true);
            }
        }

        public Task SetOnlineAsync(string userId, string? connectionId = null)
        {
            return string.IsNullOrWhiteSpace(connectionId)
                ? SetUserPresenceInDatabaseAsync(userId, isOnline: true)
                : RegisterConnectionAsync(userId, connectionId);
        }

        public async Task UnregisterConnectionAsync(string userId, string connectionId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(connectionId))
            {
                return;
            }

            if (_userConnections.TryGetValue(userId, out var connections))
            {
                connections.TryRemove(connectionId, out _);
            }
        }

        public Task SetOfflineAsync(string userId, string? connectionId = null)
        {
            return string.IsNullOrWhiteSpace(connectionId)
                ? SetUserPresenceInDatabaseAsync(userId, isOnline: false)
                : UnregisterConnectionAsync(userId, connectionId);
        }

        public async Task<bool> MarkOfflineIfNoConnectionsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            if (_userConnections.TryGetValue(userId, out var connections) && !connections.IsEmpty)
            {
                return false;
            }

            _userConnections.TryRemove(userId, out _);
            return await SetUserPresenceInDatabaseAsync(userId, isOnline: false);
        }

        private async Task<bool> SetUserPresenceInDatabaseAsync(string userId, bool isOnline)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            await using var context = await _dbFactory.CreateDbContextAsync();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                return false;
            }

            var changed = false;

            if (isOnline)
            {
                if (!user.IsOnline)
                {
                    user.IsOnline = true;
                    changed = true;
                }
            }
            else
            {
                if (user.IsOnline)
                {
                    user.IsOnline = false;
                    user.LastSeenAt = DateTime.UtcNow;
                    changed = true;
                }
            }

            if (changed)
            {
                await context.SaveChangesAsync();
            }

            return changed;
        }
    }
}
