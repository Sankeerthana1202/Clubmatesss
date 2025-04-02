using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clubmates.Web.Models.ClubsViewModel
{
    public class CustomerClubViewModel
    {
        public int ClubId { get; set; }
        public string? ClubName { get; set; }
        public string? ClubManager { get; set; }
        public string? ClubDescription { get; set; }
        public ClubCategory ClubCategory { get; set; }
        public ClubType ClubType { get; set; }
        public string? ClubRules { get; set; }
        public string? ClubManagerEmail { get; set; }

        public string? ClubContactNumber { get; set; }
        public string? ClubEmail { get; set; }
        public byte[]? ClubLogo { get; set; }
        public byte[]? ClubBanner { get; set; }
        public byte[]? ClubBackground { get; set; }
        public List<SelectListItem> ClubCategories
        {
            get
            {
                List<SelectListItem> selectListItem = Enum.GetValues<ClubCategory>()
                    .Select(x => new SelectListItem
                    {
                        Text = x.ToString(),
                        Value = x.ToString()
                    }).ToList();
                return selectListItem;
            }
        }
        public List<SelectListItem> ClubTypes
        {
            get
            {
                List<SelectListItem> selectListItem = Enum.GetValues<ClubType>()
                    .Select(x => new SelectListItem
                    {
                        Text = x.ToString(),
                        Value = x.ToString()
                    }).ToList();
                return selectListItem;
            }
        }
    }
}
