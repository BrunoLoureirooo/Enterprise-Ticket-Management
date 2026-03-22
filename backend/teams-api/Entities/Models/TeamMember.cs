namespace teams.Entities.Models
{
    public class TeamMember
    {
        public Guid TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public Guid UserId { get; set; }
        public SyncedUser? User { get; set; }
        public bool IsLeader { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
