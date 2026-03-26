using Microsoft.AspNetCore.SignalR;

namespace Kotoba.Modules.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task Register(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                Console.WriteLine($"[NotifHub] Registered: {userId}");
            }
        }
    }
}
