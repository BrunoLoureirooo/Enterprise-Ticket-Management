namespace backend.Application.Messaging
{
    public class UserChangedEvent
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty; // "Created", "Updated", "Deleted"
    }
}
