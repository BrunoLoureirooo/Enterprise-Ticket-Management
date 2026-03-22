namespace teams.Entities.DataTransferObjects.Projects
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<Guid> TeamIds { get; set; } = new();
    }
}
