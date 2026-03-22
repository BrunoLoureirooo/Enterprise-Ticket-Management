namespace teams.Entities.Models
{
    public class Project : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<ProjectTeam> ProjectTeams { get; set; } = new List<ProjectTeam>();
    }
}
