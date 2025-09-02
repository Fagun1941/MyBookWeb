using System.ComponentModel.DataAnnotations;

namespace BukyBookWeb.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email Address Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; } = false;
    }
}
