using Kotoba.Modules.Domain.DTOs;

namespace Kotoba.Modules.Domain.Interfaces;

/// <summary>
/// Service for managing user accounts and profiles.
/// </summary>
public interface IUserService
{
    Task<RegistrationResult> RegisterAsync(RegisterRequest request);
    Task<bool> LoginAsync(LoginRequest request);
    Task<UserProfile?> GetUserProfileAsync(string userId);    
    IQueryable<UserProfile> GetUsersByDisplayNameAsync(string searchValue);
    Task<AccountOperationResult> UpdateUserProfileAsync(string userId, UpdateProfileRequest request);
    Task<AccountOperationResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    Task<AccountOperationResult> DeactivateAccountAsync(string userId);
    Task<AccountOperationResult> DeleteAccountAsync(string userId);
}
