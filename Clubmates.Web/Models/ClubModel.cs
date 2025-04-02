using Clubmates.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace Clubmates.Web.Models
{
    public class ClubModel
    {
        [Key]
        public int ClubId { get; set; }

        [Required(ErrorMessage = "Club name is required.")]
        public string ClubName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Club category is required.")]
        public ClubCategory ClubCategory { get; set; }
        public string? ClubRules { get; set; }
        public string? ClubEmail { get; set; }
        public ClubType ClubType { get; set; }

        public ClubmatesUser? ClubManager { get; set; }

        [Required(ErrorMessage = "Club contact number is required.")]
        public string ClubContactNumber { get; set; } = string.Empty;

        public string? ClubDescription { get; set; }
        public byte[]? ClubLogo { get; set; }
        public byte[]? ClubBanner { get; set; }
        public byte[]? ClubBackground { get; set; }
    }

    public enum ClubCategory
    {
        Sports,
        Leisure,
        Entertainment,
        Educational,
        Research,
        Travel,
        Official,
        Government,
        Social,
        Music,
        Dance,
        Art,
        Other
    }

    public enum ClubType
    {
        Private,
        Public
    }
}