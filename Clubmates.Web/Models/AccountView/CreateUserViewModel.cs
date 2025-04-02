using Clubmates.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clubmates.Web.Models.AccountView
{
    public class CreateUserViewModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? ConfirmEmail { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public ClubmatesRole Role { get; set; }

        public List<SelectListItem> Roles
        {
            get
            {
                return Enum.GetValues<ClubmatesRole>()
                    .Select(x => new SelectListItem
                    {
                        Text = Enum.GetName(x),
                        Value = x.ToString()
                    }).ToList();

            }
        }
    }
}