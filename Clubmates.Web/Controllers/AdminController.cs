using Clubmates.Web.AppDbContext;
using Clubmates.Web.Models;
using Clubmates.Web.Models.AccountView;
using Clubmates.Web.Models.AdminViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Clubmates.Web.Controllers
{
    [Authorize(Policy = "MustbeSuperAdmin")]
    public class AdminController(UserManager<ClubmatesUser> userManager, AppIdentityDbContext context) : Controller
    {
        private readonly UserManager<ClubmatesUser> _userManager = userManager;
        private readonly AppIdentityDbContext _dbContext = context;
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ManageUsers()
        {
            return View(await GetUsersToManageAsync());

        }
        private async Task<List<UserViewModel>> GetUsersToManageAsync()
        {
            var User = await _userManager.Users
            .Where(x => x.ClubmatesRole != ClubmatesRole.SuperAdmin).ToListAsync();

            var listofUserAccounts = new List<UserViewModel>();
            foreach (var user in User)
            {
                listofUserAccounts.Add(new UserViewModel
                {
                    Email = user.Email,
                    Name = await GetNameForUser(user.Email ?? string.Empty),
                    Role = user.ClubmatesRole
                });
            }

            return listofUserAccounts;

        }

        private async Task<string> GetNameForUser(string Email)
        {
            var accountuser = await _userManager.FindByEmailAsync(Email);
            if (accountuser != null)
            {
                var claims = await _userManager.GetClaimsAsync(accountuser);
                if (claims != null)
                {
                    return claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty;
                }

            }
            return String.Empty 
;
        }


        public async Task<IActionResult> EditUser(string email)
        {
            var accountUser = await _userManager.FindByEmailAsync(email);

            if (accountUser != null)
            {
                UserViewModel userViewModel = new UserViewModel()
                {
                    Email = accountUser.Email,
                    Name = await GetNameForUser(accountUser.Email ?? string.Empty),
                    Role = accountUser.ClubmatesRole,
                    Roles = Enum.GetValues<ClubmatesRole>()
                    .Select(x => new SelectListItem
                    {
                        Text = Enum.GetName(x),
                        Value = x.ToString()
                    })
                };
                return View(userViewModel);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(userViewModel.Email))
                    {
                        ClubmatesUser? clubmatesUser = await _userManager.FindByEmailAsync(userViewModel.Email);
                        if (clubmatesUser != null)
                        {
                            clubmatesUser.ClubmatesRole = userViewModel.Role;
                            var claims = await _userManager.GetClaimsAsync(clubmatesUser);
                            var removeResult = await _userManager.RemoveClaimsAsync(clubmatesUser, claims);

                            if (!removeResult.Succeeded)
                            {
                                ModelState.AddModelError(string.Empty, "Unable to update claim-removing existing claim");
                                return View(userViewModel);
                            }

                            var claimsRequired = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,userViewModel.Name ?? " "),
                            new Claim(ClaimTypes.Role,Enum.GetName(userViewModel.Role)?? " "),
                            new Claim(ClaimTypes.NameIdentifier,clubmatesUser.Id),
                            new Claim(ClaimTypes.Email,userViewModel.Email)
                        };
                            var addclaimResult = await _userManager.AddClaimsAsync(clubmatesUser, claimsRequired);
                            if (!addclaimResult.Succeeded)
                            {
                                ModelState.AddModelError(string.Empty, "Unable to update claim - adding new claim failed");
                                return View(userViewModel);
                            }
                            var userUpdateResult = await _userManager.UpdateAsync(clubmatesUser);
                            if (!userUpdateResult.Succeeded)
                            {
                                ModelState.AddModelError("", "Failed to update user");
                                return View(userViewModel);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(userViewModel);

                }

            }
            return RedirectToAction("ManageUsers", await GetUsersToManageAsync());
        }


        public async Task<IActionResult> DeleteUser(string email)
        {
            var accountUser = await _userManager.FindByEmailAsync(email);
            if (accountUser != null)
            {
                await _userManager.DeleteAsync(accountUser);
                return View("ManageUsers", await GetUsersToManageAsync());
            }
            return NotFound();
        }
        public IActionResult CreateUser()
        {
            return View(new CreateUserViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel createUserViewModel)

        {

            if (!ModelState.IsValid)

            {

                return View(createUserViewModel);

            }

            if (createUserViewModel.Email != createUserViewModel.ConfirmEmail)

            {

                ModelState.AddModelError(string.Empty, "Email and ConfirmEmail do not match");

                return View(createUserViewModel);

            }

            if (createUserViewModel.Password != createUserViewModel.ConfirmPassword)

            {

                ModelState.AddModelError(string.Empty, "Password and ConfirmPassword do not match");

                return View(createUserViewModel);

            }

            ClubmatesUser clubmatesUser = new()

            {

                Email = createUserViewModel.Email,

                UserName = createUserViewModel.Name,

                ClubmatesRole = createUserViewModel.Role

            };

            var createdUser = await _userManager.CreateAsync(clubmatesUser, createUserViewModel.Password ?? " ");

            if (!createdUser.Succeeded)

            {

                foreach (var error in createdUser.Errors)

                {

                    ModelState.AddModelError(string.Empty, error.Description);

                }

                return View(createUserViewModel);

            }

            var claimRequired = new List<Claim>

{

    new Claim(ClaimTypes.Name, createUserViewModel.Name??" "),

    new Claim(ClaimTypes.Role, Enum.GetName(createUserViewModel.Role) ?? string.Empty),

    new Claim(ClaimTypes.NameIdentifier, clubmatesUser.Id),

    new Claim(ClaimTypes.Email, createUserViewModel.Email?? "")

};

            var claimResult = await _userManager.AddClaimsAsync(clubmatesUser, claimRequired);

            await _userManager.UpdateAsync(clubmatesUser);

            if (!claimResult.Succeeded)

            {

                foreach (var error in claimResult.Errors)

                {

                    ModelState.AddModelError(string.Empty, error.Description);

                }

                return View(createUserViewModel);

            }

            return RedirectToAction("ManageUsers", await GetUsersToManageAsync());

        }
        public async Task<IActionResult> ManageClubs()
        {
            var listOfClubs = await _dbContext.Clubs
                        .Include(x => x.ClubManager)
                        .ToListAsync();
            List<ClubViewModel> clubViewModels = listOfClubs.Select(x => new ClubViewModel
            {
                ClubId = x.ClubId,
                ClubName = x.ClubName,
                ClubEmail = x.ClubEmail,
                ClubCategory = x.ClubCategory,
                ClubType = x.ClubType,
                ClubManagerEmail = x.ClubManager?.Email,
                ClubContactNumber = x.ClubContactNumber,
                ClubDescription = x.ClubDescription
            }).ToList();
            return View(clubViewModels);
        }

        public IActionResult CreateClub()
        {
            return View(new ClubViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateClub(ClubViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var club = new ClubModel
                {
                    ClubName = model.ClubName ?? "",
                    ClubType = model.ClubType,
                    ClubCategory = model.ClubCategory,
                    ClubDescription = model.ClubDescription,
                    ClubManager = await _userManager.FindByEmailAsync(model.ClubManagerEmail ?? string.Empty),
                    ClubEmail = model.ClubEmail,
                    ClubRules = model.ClubRules,
                    ClubContactNumber = model.ClubContactNumber ?? "",
                };

                _dbContext.Clubs.Add(club);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("ManageClubs");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }


        }

        public async Task<IActionResult> EditClub(int clubId)
        {
            var club = await _dbContext
                        .Clubs
                        .Include(x => x.ClubManager)
                        .FirstOrDefaultAsync(x => x.ClubId == clubId);

            if (club == null)
            {
                return NotFound();
            }

            var clubViewModel = new ClubViewModel()
            {
                ClubId = club.ClubId,
                ClubName = club.ClubName,
                ClubEmail = club.ClubEmail,
                ClubCategory = club.ClubCategory,
                ClubType = club.ClubType,
                ClubManagerEmail = club.ClubManager?.Email,
                ClubRules = club.ClubRules,
                ClubContactNumber = club.ClubContactNumber,
                ClubDescription = club.ClubDescription,
            };

            return View(clubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditClub(ClubViewModel clubViewModel)
        {
            var club = await _dbContext.Clubs.FindAsync(clubViewModel.ClubId);
            if (club != null)
            {
                club.ClubName = clubViewModel.ClubName ?? "";
                club.ClubEmail = clubViewModel.ClubEmail;
                club.ClubCategory = clubViewModel.ClubCategory;
                club.ClubType = clubViewModel.ClubType;
                club.ClubRules = clubViewModel.ClubRules;
                club.ClubManager = await _userManager.FindByEmailAsync(clubViewModel.ClubManagerEmail ?? string.Empty);
                club.ClubContactNumber = clubViewModel.ClubContactNumber ?? "";
                club.ClubDescription = clubViewModel.ClubDescription;

                _dbContext.Clubs.Update(club);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("ManageClubs");
            }

            return NotFound();
        }


        public async Task<IActionResult> DeleteClub(int clubId)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("ManageClub");
            }
            var club = await _dbContext.Clubs.FindAsync(clubId);
            if (club != null)
            {
                _dbContext.Remove(club);
                await _dbContext.SaveChangesAsync();

                return Redirect("ManageClubs");
            }
            return NotFound();
        }


    }
}
