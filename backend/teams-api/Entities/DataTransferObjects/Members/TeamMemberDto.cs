namespace teams.Entities.DataTransferObjects.Members
{
    public class TeamMemberDto
    {
        public Guid UserId { get; set; }
        public bool IsLeader { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
