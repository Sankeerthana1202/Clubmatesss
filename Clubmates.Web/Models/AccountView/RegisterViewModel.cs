using System.ComponentModel.DataAnnotations;

namespace Clubmates.Web.Models.AccountView
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="FullName is mandatory")]
        public string FullName { get; set; } = string.Empty;
        [Required(ErrorMessage ="Email Id is mandatory")]
        [EmailAddress(ErrorMessage ="This is not a valid email address,use this format test@test.com")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage ="Please provide a password to secure your account")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage ="Provide password same as previous")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
