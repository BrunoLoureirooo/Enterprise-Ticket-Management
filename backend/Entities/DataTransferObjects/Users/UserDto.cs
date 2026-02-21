
namespace backend.Entities.DataTransferObjects.Users
{
    public record UserDto
    {
        public required string Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public string? ImageUrl { get; set; }
    }
}
