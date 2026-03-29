using Kotoba.Modules.Domain.Interfaces;

namespace Kotoba.Modules.Infrastructure.Services.Social
{
    public class FollowService : IFollowService
    {
        private readonly IFollowRepository _followRepository;

        public FollowService(IFollowRepository followRepository)
        {
            _followRepository = followRepository;
        }

        public async Task<bool> FollowUserAsync(string followerId, string followingId)
        {
            if (followerId == followingId)
                return false; 

            return await _followRepository.FollowAsync(followerId, followingId);
        }

        public async Task<bool> UnfollowUserAsync(string followerId, string followingId)
        {
            return await _followRepository.UnfollowAsync(followerId, followingId);
        }

        public async Task<bool> IsFollowingAsync(string followerId, string followingId)
        {
            return await _followRepository.IsFollowingAsync(followerId, followingId);
        }

        public async Task<List<string>> GetFollowingIdsAsync(string userId)
        {
            return await _followRepository.GetFollowingIdsAsync(userId);
        }
    }
}
