namespace ticket.Entities.DataTransferObjects.Tickets
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public Guid CreatedById { get; set; }
        public Guid? AssignedToId { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }
}
