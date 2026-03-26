namespace Kotoba.Modules.Infrastructure.Services.Notifications;

/// <summary>
/// Scoped service — populated from the HTTP middleware BEFORE the Blazor
/// circuit starts, then read by GlobalNotificationService later.
/// </summary>
public class CircuitCookieService
{
    public string CookieHeader { get; set; } = string.Empty;
}
