using System.ComponentModel.DataAnnotations;

namespace backend.Entities.DataTransferObjects.Users
{
    public record UserForAuthenticationDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
