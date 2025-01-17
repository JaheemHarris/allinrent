using System.ComponentModel.DataAnnotations;

namespace RazorAllinRent.Dto
{
    public class LoginFormDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
