using Kotoba.Modules.Domain.DTOs;
using Kotoba.Modules.Domain.Entities;
using Kotoba.Modules.Domain.Interfaces;
using Kotoba.Modules.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Kotoba.Modules.Infrastructure.Services.Identity
{
    public class UserService : IUserService
    {
        private const string ProfileTokenProvider = "Kotoba.Profile";
        private const string ProfileBioTokenName = "Bio";

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly UserProfileRepository _userProfileRepository;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, UserProfileRepository userProfileRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userProfileRepository = userProfileRepository;
        }

        public Task<UserProfile?> GetUserProfileAsync(string userId)
        {
            return GetUserProfileInternalAsync(userId);
        }

        public IQueryable<UserProfile> GetUsersByDisplayNameAsync(string searchValue)
        {
            return _userProfileRepository.GetUsersByDisplayNameAsync(searchValue);
        }

        public async Task<bool> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return false;
            }

            var normalizedEmail = request.Email.Trim();
            var user = await _userManager.FindByEmailAsync(normalizedEmail);
            if (user is null)
            {
                return false;
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                request.Password,
                isPersistent: true,
                lockoutOnFailure: false);

            return result.Succeeded;
        }

        public async Task<RegistrationResult> RegisterAsync(RegisterRequest request)
        {
            var validationErrors = new List<string>();
            if (string.IsNullOrWhiteSpace(request.DisplayName))
            {
                validationErrors.Add("Display name is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                validationErrors.Add("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                validationErrors.Add("Password is required.");
            }

            if (validationErrors.Count > 0)
            {
                return RegistrationResult.Failure(validationErrors);
            }

            var normalizedEmail = request.Email.Trim();
            var existingUser = await _userManager.FindByEmailAsync(normalizedEmail);
            if (existingUser is not null)
            {
                return RegistrationResult.Failure(new[] { "Email is already in use." });
            }

            var user = new User
            {
                UserName = normalizedEmail,
                Email = normalizedEmail,
                DisplayName = request.DisplayName.Trim(),
                IsOnline = false,
                LastSeenAt = null,
                CreatedAt = DateTime.UtcNow
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var identityErrors = createResult.Errors
                    .Select(error => error.Description)
                    .Where(description => !string.IsNullOrWhiteSpace(description))
                    .ToList();

                return RegistrationResult.Failure(identityErrors);
            }

            await _signInManager.SignInAsync(user, isPersistent: true);
            return RegistrationResult.Success();
        }

        public async Task<AccountOperationResult> UpdateUserProfileAsync(string userId, UpdateProfileRequest request)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(request.DisplayName))
            {
                return AccountOperationResult.Failure(new[] { "Display name is required." });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return AccountOperationResult.Failure(new[] { "User profile not found." });
            }

            user.DisplayName = request.DisplayName.Trim();
            user.AvatarUrl = string.IsNullOrWhiteSpace(request.AvatarUrl) ? null : request.AvatarUrl.Trim();

            var requestedUserName = string.IsNullOrWhiteSpace(request.UserName)
                ? user.UserName ?? user.Email ?? user.Id
                : request.UserName.Trim().TrimStart('@');

            var requestedEmail = string.IsNullOrWhiteSpace(request.Email)
                ? user.Email ?? string.Empty
                : request.Email.Trim();

            if (!string.Equals(user.UserName, requestedUserName, StringComparison.Ordinal))
            {
                var setUserNameResult = await _userManager.SetUserNameAsync(user, requestedUserName);
                if (!setUserNameResult.Succeeded)
                {
                    return FromIdentityResult(setUserNameResult);
                }
            }

            if (!string.Equals(user.Email, requestedEmail, StringComparison.OrdinalIgnoreCase))
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, requestedEmail);
                if (!setEmailResult.Succeeded)
                {
                    return FromIdentityResult(setEmailResult);
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return FromIdentityResult(updateResult);
            }

            if (string.IsNullOrWhiteSpace(request.Bio))
            {
                await _userManager.RemoveAuthenticationTokenAsync(user, ProfileTokenProvider, ProfileBioTokenName);
            }
            else
            {
                await _userManager.SetAuthenticationTokenAsync(user, ProfileTokenProvider, ProfileBioTokenName, request.Bio.Trim());
            }

            return AccountOperationResult.Success();
        }

        public async Task<AccountOperationResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return AccountOperationResult.Failure(new[] { "Unable to resolve current user." });
            }

            if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                return AccountOperationResult.Failure(new[] { "Current and new password are required." });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return AccountOperationResult.Failure(new[] { "User profile not found." });
            }

            var changeResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!changeResult.Succeeded)
            {
                return FromIdentityResult(changeResult);
            }

            return AccountOperationResult.Success();
        }

        public async Task<AccountOperationResult> DeactivateAccountAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return AccountOperationResult.Failure(new[] { "Unable to resolve current user." });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return AccountOperationResult.Failure(new[] { "User profile not found." });
            }

            user.IsOnline = false;
            user.LastSeenAt = DateTime.UtcNow;
            user.LockoutEnabled = true;

            var lockoutResult = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            if (!lockoutResult.Succeeded)
            {
                return FromIdentityResult(lockoutResult);
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return FromIdentityResult(updateResult);
            }

            await _signInManager.SignOutAsync();
            return AccountOperationResult.Success();
        }

        public async Task<AccountOperationResult> DeleteAccountAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return AccountOperationResult.Failure(new[] { "Unable to resolve current user." });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return AccountOperationResult.Failure(new[] { "User profile not found." });
            }

            var deleteResult = await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
                return FromIdentityResult(deleteResult);
            }

            await _signInManager.SignOutAsync();
            return AccountOperationResult.Success();
        }

        private async Task<UserProfile?> GetUserProfileInternalAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return null;
            }

            var bio = await _userManager.GetAuthenticationTokenAsync(user, ProfileTokenProvider, ProfileBioTokenName);

            return new UserProfile
            {
                UserId = user.Id,
                DisplayName = user.DisplayName,
                AvatarUrl = user.AvatarUrl,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Bio = bio ?? string.Empty,
                IsOnline = user.IsOnline,
                LastSeenAt = user.LastSeenAt
            };
        }


        private static AccountOperationResult FromIdentityResult(IdentityResult result)
        {
            if (result.Succeeded)
            {
                return AccountOperationResult.Success();
            }

            var errors = result.Errors
                .Select(error => error.Description)
                .Where(description => !string.IsNullOrWhiteSpace(description))
                .ToList();

            if (errors.Count == 0)
            {
                errors.Add("Operation failed.");
            }

            return AccountOperationResult.Failure(errors);
        }
    }
}
