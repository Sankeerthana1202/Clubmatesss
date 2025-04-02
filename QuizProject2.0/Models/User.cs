using Microsoft.AspNetCore.Identity;

namespace QuizProject2._0.Models
{
    public class User:IdentityUser
    {
        public Role Role { get; set; }

    }
    public enum Role
    {
        User,
        Guest,
        Admin,
        SuperAdmin
    }
}
