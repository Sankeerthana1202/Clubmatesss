using System.ComponentModel.DataAnnotations;

namespace QuizProject2._0.Models.AccountView
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is mandatory")]
        public String UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is mandatory")]
        public string Password { get; set; } = string.Empty;
    }
}
