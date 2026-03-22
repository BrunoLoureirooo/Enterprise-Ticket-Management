namespace teams.Entities.Models
{
    public class Team : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
        public ICollection<ProjectTeam> ProjectTeams { get; set; } = new List<ProjectTeam>();
    }
}
