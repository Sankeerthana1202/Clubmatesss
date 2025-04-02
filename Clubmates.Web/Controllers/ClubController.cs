using Clubmates.Web.AppDbContext;
using Clubmates.Web.Models.AdminViewModel;
using Clubmates.Web.Models.ClubsViewModel;
using Clubmates.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Clubmates.Web.Controllers
{
    [Authorize(Policy = "MustbeGuest")]
    public class ClubController : Controller
    {
        private readonly UserManager<ClubmatesUser> _userManager;
        private readonly AppIdentityDbContext _dbContext;

        public ClubController(AppIdentityDbContext dbContext, UserManager<ClubmatesUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var listOfClubs = await _dbContext.Clubs.ToListAsync();

            var listOfClubsViewModel = listOfClubs.Select(club => new CustomerClubViewModel
            {
                ClubId = club.ClubId,
                ClubName = club.ClubName,
                ClubDescription = club.ClubDescription,
                ClubType = club.ClubType,
                ClubRules = club.ClubRules,
                ClubManagerEmail = club.ClubManager?.Email,
                ClubContactNumber = club.ClubContactNumber,
                ClubEmail = club.ClubEmail,
                ClubLogo = club.ClubLogo,
                ClubBanner = club.ClubBanner,
                ClubBackground = club.ClubBackground
            }).ToList();
            return View(listOfClubsViewModel);
        }

        public async Task<IActionResult> ClubDetails(int clubId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            

           
            var clubDetails = await _dbContext.Clubs.Include(x => x.ClubManager).FirstOrDefaultAsync(y => y.ClubId == clubId);
            if (clubDetails == null)
            {
                return RedirectToAction("Index");
            }
            var clubDetailsViewModel = new CustomerClubViewModel
            {
                ClubName = clubDetails.ClubName,
                ClubDescription = clubDetails.ClubDescription,
                ClubType = clubDetails.ClubType,
                ClubRules = clubDetails.ClubRules,
                ClubManagerEmail = clubDetails.ClubManager?.Email,
                ClubContactNumber = clubDetails.ClubContactNumber,
                ClubEmail = clubDetails.ClubEmail,
                ClubLogo = clubDetails.ClubLogo,
                ClubBanner = clubDetails.ClubBanner,
                ClubBackground = clubDetails.ClubBackground,
            };
            return View(clubDetailsViewModel);
        }

        public IActionResult CreateClubForCustomer()
        {

            return View(new CustomerClubViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> CreateClubForCustomer(CustomerClubViewModel customerClubViewModel, IFormFile clublogo, IFormFile clubBanner, IFormFile clubBackground)
        {
            if (!ModelState.IsValid)
            {
                return View(customerClubViewModel);
            }

            var loggedInUserEmail = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (loggedInUserEmail == null) { return RedirectToAction("Login", "Account", new { returnUrl = "/Club/CreateClubForCustomer" }); }
            var loggedInUser = await _userManager.FindByEmailAsync(loggedInUserEmail);
            if (customerClubViewModel != null && loggedInUser != null)
            {


                var claims = new List<Claim>
            {
                new(ClaimTypes.Role,"ClubManager"),
                new("ClubId",customerClubViewModel.ClubId.ToString()),
                new("ClubName",customerClubViewModel.ClubName ?? ""),
                new(ClaimTypes.Role,"Guest") };



                await _userManager.AddClaimsAsync(loggedInUser, claims);
           
                
                await _userManager.UpdateAsync(loggedInUser);
                ClubModel club = new()
                {
                    ClubName = customerClubViewModel.ClubName,
                    ClubDescription = customerClubViewModel.ClubDescription,
                    ClubCategory = customerClubViewModel.ClubCategory,
                    ClubType = customerClubViewModel.ClubType,
                    ClubRules = customerClubViewModel.ClubRules,
                    ClubManager = loggedInUser,
                    ClubContactNumber = customerClubViewModel.ClubContactNumber,
                    ClubEmail = customerClubViewModel.ClubEmail

                };
                if (clublogo != null)
                {
                    using var memoryStream = new MemoryStream();
                    await clublogo.CopyToAsync(memoryStream);
                    club.ClubLogo = memoryStream.ToArray();


                }
                if (clubBanner != null)
                {
                    using var memoryStream = new MemoryStream();
                    await clubBanner.CopyToAsync(memoryStream);
                    club.ClubBanner = memoryStream.ToArray();


                }
                if (clubBackground != null)
                {
                    using var memoryStream = new MemoryStream();
                    await clubBackground.CopyToAsync(memoryStream);
                    club.ClubBackground = memoryStream.ToArray();


                }
                _dbContext.Clubs.Add(club);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View("Index","Club");


        }

    }
}
