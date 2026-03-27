using Kotoba.Modules.Domain.Interfaces;
using Kotoba.Modules.Domain.DTOs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Kotoba.Modules.Hubs
{
    public class NotificationHub : Hub
    {
        private static readonly TimeSpan OfflineGracePeriod = TimeSpan.FromMinutes(1);
        private static readonly ConcurrentDictionary<string, string> ConnectionUsers = new();

        private readonly IPresenceService _presenceService;
        private readonly IPresenceBroadcastService _presenceBroadcastService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationHub(
            IPresenceService presenceService,
            IPresenceBroadcastService presenceBroadcastService,
            IHubContext<NotificationHub> hubContext)
        {
            _presenceService = presenceService;
            _presenceBroadcastService = presenceBroadcastService;
            _hubContext = hubContext;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectionUsers.TryRemove(Context.ConnectionId, out var userId);
            if (!string.IsNullOrWhiteSpace(userId))
            {
                await _presenceService.UnregisterConnectionAsync(userId, Context.ConnectionId);

                _ = HandleDelayedOfflineAsync(userId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task Register(string? userId)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                ConnectionUsers[Context.ConnectionId] = userId;
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);

                var wasOnline = await _presenceService.GetUserPresenceAsync(userId);
                await _presenceService.RegisterConnectionAsync(userId, Context.ConnectionId);

                if (!wasOnline)
                {
                    var update = await _presenceBroadcastService.NotifyUserOnlineAsync(userId);
                    await Clients.All.SendAsync("PresenceChanged", update);
                    await Clients.All.SendAsync("UserOnline", userId);
                }

                var onlineUsers = await _presenceBroadcastService.GetAllOnlineUsersAsync();
                await Clients.Caller.SendAsync("OnlineUsersSnapshot", onlineUsers);

                Console.WriteLine($"[NotifHub] Registered: {userId}");
            }
        }

        public async Task Unregister(string? userId)
        {
            var resolvedUserId = userId;
            if (string.IsNullOrWhiteSpace(resolvedUserId))
            {
                ConnectionUsers.TryGetValue(Context.ConnectionId, out resolvedUserId);
            }

            ConnectionUsers.TryRemove(Context.ConnectionId, out _);

            if (!string.IsNullOrWhiteSpace(resolvedUserId))
            {
                await _presenceService.UnregisterConnectionAsync(resolvedUserId, Context.ConnectionId);
                _ = HandleDelayedOfflineAsync(resolvedUserId);
            }
        }

        private async Task HandleDelayedOfflineAsync(string userId)
        {
            try
            {
                await Task.Delay(OfflineGracePeriod);

                var turnedOffline = await _presenceService.MarkOfflineIfNoConnectionsAsync(userId);
                if (!turnedOffline)
                {
                    return;
                }

                var update = new PresenceUpdateDto
                {
                    UserId = userId,
                    DisplayName = userId,
                    IsOnline = false,
                    LastSeenAt = DateTime.UtcNow,
                    Timestamp = DateTime.UtcNow
                };
                await _hubContext.Clients.All.SendAsync("PresenceChanged", update);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[NotifHub] Presence offline update failed: {ex.Message}");
            }
        }
    }
}
