namespace teams.Entities.Events
{
    public class MembershipChangedEvent
    {
        public Guid UserId { get; set; }
        public Guid TeamId { get; set; }
        public bool IsLeader { get; set; }
        public string Action { get; set; } = string.Empty; // "Added", "Removed", "Updated"
    }
}
