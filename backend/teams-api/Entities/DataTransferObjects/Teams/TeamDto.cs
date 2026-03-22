using teams.Entities.DataTransferObjects.Members;

namespace teams.Entities.DataTransferObjects.Teams
{
    public class TeamDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<TeamMemberDto> Members { get; set; } = new();
    }
}
