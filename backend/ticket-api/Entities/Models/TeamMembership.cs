namespace ticket.Entities.Models
{
    public class TeamMembership
    {
        public Guid UserId { get; set; }
        public Guid TeamId { get; set; }
        public bool IsLeader { get; set; }
    }
}
