namespace teams.Entities.Events
{
    public class TeamChangedEvent
    {
        public Guid TeamId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty; // "Created", "Updated", "Deleted"
    }
}
