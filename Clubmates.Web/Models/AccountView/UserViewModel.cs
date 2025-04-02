using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clubmates.Web.Models.AccountView
{
    public class UserViewModel
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public ClubmatesRole Role { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; } = [];
    }
}
