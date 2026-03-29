namespace Kotoba.Modules.Domain.Interfaces
{
    public interface IFollowRepository
    {
        Task<bool> FollowAsync(string followerId, string followingId);
        Task<bool> UnfollowAsync(string followerId, string followingId);
        Task<bool> IsFollowingAsync(string followerId, string followingId);
        Task<List<string>> GetFollowingIdsAsync(string userId);
    }
}
