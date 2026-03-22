namespace backend.Entities.Models
{
    public class SyncedTeamMembership
    {
        public Guid UserId { get; set; }
        public Guid TeamId { get; set; }
        public bool IsLeader { get; set; }
    }
}
