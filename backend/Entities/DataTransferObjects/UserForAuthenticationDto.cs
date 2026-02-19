using System.ComponentModel.DataAnnotations;

namespace backend.Entities.DataTransferObjects
{
    public record UserForAuthenticationDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
