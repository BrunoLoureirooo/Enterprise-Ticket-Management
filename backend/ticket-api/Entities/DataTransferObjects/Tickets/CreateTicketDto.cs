namespace ticket.Entities.DataTransferObjects.Tickets
{
    public class CreateTicketDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Priority { get; set; } = "Medium";
        public Guid? AssignedToId { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? ProjectId { get; set; }
    }
}
