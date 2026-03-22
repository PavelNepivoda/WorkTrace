using System.ComponentModel.DataAnnotations;

namespace WorkTrace.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Zapamatovat si mě")]
        public bool RememberMe { get; set; }
    }
}