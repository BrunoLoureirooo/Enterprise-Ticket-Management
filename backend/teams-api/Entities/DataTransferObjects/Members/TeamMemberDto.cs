namespace teams.Entities.DataTransferObjects.Members
{
    public class TeamMemberDto
    {
        public Guid UserId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool IsLeader { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
