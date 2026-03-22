namespace teams.Entities.DataTransferObjects.Members
{
    public class AddMemberDto
    {
        public Guid UserId { get; set; }
        public bool IsLeader { get; set; }
    }
}
