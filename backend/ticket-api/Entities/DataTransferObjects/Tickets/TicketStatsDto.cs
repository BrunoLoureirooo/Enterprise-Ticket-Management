namespace ticket.Entities.DataTransferObjects.Tickets
{
    public class TicketStatsDto
    {
        public List<StatusCountDto> UnresolvedByStatus { get; set; } = [];
        public List<PriorityCountDto> ByPriority { get; set; } = [];
        public List<PriorityCountDto>? TeamUnresolvedByPriority { get; set; }
        public bool IsTeamLeader { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class StatusCountDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class PriorityCountDto
    {
        public string Priority { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
