using ticket.Entities.Enums;

namespace ticket.Entities.Models
{
    public class Ticket : BaseModel
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TicketStatus Status { get; set; } = TicketStatus.Open;
        public TicketPriority Priority { get; set; } = TicketPriority.Medium;
        public Guid CreatedById { get; set; }
        public Guid? AssignedToId { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? ProjectId { get; set; }
        public DateTime? ClosedAt { get; set; }
    }
}
