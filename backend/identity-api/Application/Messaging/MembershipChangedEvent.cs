namespace backend.Application.Messaging
{
    public record MembershipChangedEvent
    {
        public Guid UserId { get; init; }
        public Guid TeamId { get; init; }
        public bool IsLeader { get; init; }
        public string Action { get; init; } = string.Empty;
    }
}
