namespace Kotoba.Modules.Domain.Interfaces
{
    public interface IFollowService
    {
        Task<bool> FollowUserAsync(string followerId, string followingId);
        Task<bool> UnfollowUserAsync(string followerId, string followingId);
        Task<bool> IsFollowingAsync(string followerId, string followingId);
        Task<List<string>> GetFollowingIdsAsync(string userId);
    }
}
