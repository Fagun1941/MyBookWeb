using System.ComponentModel.DataAnnotations;

namespace BukyBookWeb.Models
{
    public class Login
    {
        [Required]
        public required string PhoneNumber { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
