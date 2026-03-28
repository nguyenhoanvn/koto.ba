namespace Kotoba.Modules.Domain.Enums
{
    public enum NotificationType
    {
        NewMessage,
        NewFollower,        // User A bắt đầu follow bạn
        StoryReaction,      // User A react story của bạn
        AdminWarning,       // Admin cảnh báo bạn vì bị report
        AdminReportAlert    // (Admin only) có user bị report
    }
}
