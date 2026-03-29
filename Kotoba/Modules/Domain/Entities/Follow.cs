namespace Kotoba.Modules.Domain.Entities
{
    public class Follow
    {
        public string FollowerId { get; set; } = null!;   // người follow
        public string FollowingId { get; set; } = null!;  // người bị follow

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User Follower { get; set; } = null!;
        public User Following { get; set; } = null!;
    }
}
