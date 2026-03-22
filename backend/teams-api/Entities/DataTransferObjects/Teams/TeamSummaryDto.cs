namespace teams.Entities.DataTransferObjects.Teams
{
    public class TeamSummaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
