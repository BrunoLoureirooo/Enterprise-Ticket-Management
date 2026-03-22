namespace teams.Entities.DataTransferObjects.Projects
{
    public class CreateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<Guid> TeamIds { get; set; } = new();
    }
}
