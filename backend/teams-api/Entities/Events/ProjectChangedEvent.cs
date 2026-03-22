namespace teams.Entities.Events
{
    public class ProjectChangedEvent
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Guid> TeamIds { get; set; } = new();
        public string Action { get; set; } = string.Empty; // "Created", "Updated", "Deleted"
    }
}
