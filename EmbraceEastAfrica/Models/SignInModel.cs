using System.ComponentModel.DataAnnotations;

namespace EmbraceEastAfrica.Models
{
    /// <summary>
    /// View model for the Sign In form.
    /// </summary>
    public class SignInModel
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; } = string.Empty;
    }
}
