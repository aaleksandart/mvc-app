using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc_app_login.Models.Forms;
using mvc_app_login.Models.Profiles;
using mvc_app_login.Services;

namespace mvc_app_login.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserProfileService _userProfileService;
        private readonly AppDbContext _context;

        public AdminController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IUserProfileService userProfileService, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userProfileService = userProfileService;
            _context = context;
        }


        #region Admin-UserProfile

        [HttpGet]
        [Route("admin/users")]
        public async Task<IActionResult> Users()
        {
            var usersList = await _context.UserProfiles
                .Include(x => x.UserFromIdentity)
                .ToListAsync();

            return View(usersList);
        }

        [HttpGet("{id}")]
        [Route("admin/userprofile/{id}")]
        public async Task<IActionResult> UserProfile(string id)
        {
            var displayUserProfile = await _userProfileService.DisplayProfileAsync(id);
            return View(displayUserProfile);
        }

        [HttpGet]
        [Route("admin/update-userprofile")]
        public IActionResult UserUpdate(string id, string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var updateforms = new AdminUpdateForm();
                updateforms.Email = id;
                if (returnUrl != null)
                    updateforms.ReturnUrl = returnUrl;

                return View(updateforms);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPut("{id}")]
        [Route("admin/update-userprofile")]
        public async Task<IActionResult> PostUserUpdate(string id, AdminUpdateForm updateform)
        {
            if (ModelState.IsValid)
            {
                var updateProfile = new UpdateProfileModel
                {
                    FirstName = updateform.FirstName,
                    LastName = updateform.LastName,
                    StreetName = updateform.StreetName,
                    PostalCode = updateform.PostalCode,
                    City = updateform.City
                };
                var updateSuccess = await _userProfileService.UpdateProfileAsync(id, updateProfile);
                if (updateSuccess.Success)
                {
                    if (updateform.ReturnUrl == null || updateform.ReturnUrl == "/")
                        return RedirectToAction("Index", "Home");
                    else
                        return LocalRedirect(updateform.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "NotFound");
                }
            }
            else
            {
                updateform.ErrorMessage = "Failed to update. Try again!";
            }
            return View(updateform);
        }

        [HttpDelete("{id}")]
        [Route("admin/delete-userprofile")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleteUserFromIdentity = await _userManager.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (User.Identity.Name == deleteUserFromIdentity.Email)
                await _signInManager.SignOutAsync();

            var deleteSuccess = await _userProfileService.DeleteProfileAsync(id);
            if (deleteSuccess.Success && deleteUserFromIdentity != null)
            {
                _context.Users.Remove(deleteUserFromIdentity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            
            else
                return RedirectToAction("Index", "NotFound");
        }

        #endregion

        #region Admin-Roles

        [HttpGet]
        [Route("admin/roles")]
        public async Task <IActionResult> Roles()
        {
            var rolesList = await _context.Roles.ToListAsync();
            return View(rolesList);
        }

        [HttpGet]
        [Route("admin/create-role")]
        public IActionResult CreateRole(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var createforms = new AdminRoleForm();
                if (returnUrl != null)
                    createforms.ReturnUrl = returnUrl;

                return View(createforms);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("admin/create-role")]
        public async Task<IActionResult> CreateRole(AdminRoleForm createform)
        {
            if (ModelState.IsValid)
            {
                var existingRole = await _context.Roles
                    .Where(x => x.Name == createform.Name)
                    .FirstOrDefaultAsync();

                if (existingRole == null)
                {
                    var newRole = await _roleManager.CreateAsync(new IdentityRole(createform.Name));
                    if (newRole != null)
                    {
                        _context.SaveChanges();
                        if (createform.ReturnUrl == null || createform.ReturnUrl == "/")
                            return RedirectToAction("Index", "Home");
                        else
                            return LocalRedirect(createform.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "NotFound");
                    }
                }
                else
                    createform.ErrorMessage = "That role already exists.";
            }
            else
            {
                createform.ErrorMessage = "Failed to update. Try again!";
            }
            return View(createform);
        }


        [HttpGet]
        [Route("admin/update-role")]
        public IActionResult UpdateRole(string id, string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var updateforms = new AdminRoleForm();
                updateforms.Id = id;
                if (returnUrl != null)
                    updateforms.ReturnUrl = returnUrl;

                return View(updateforms);
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpPut("{id}")]
        [Route("admin/update-role")]
        public async Task<IActionResult> PutUpdateRole(string id, AdminRoleForm updateform)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    role.Name = updateform.Name;
                    _context.Entry(role).State = EntityState.Modified;
                    _context.SaveChanges();
                    if (updateform.ReturnUrl == null || updateform.ReturnUrl == "/")
                        return RedirectToAction("Index", "Home");
                    else
                        return LocalRedirect(updateform.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "NotFound");
                }
            }
            else
            {
                updateform.ErrorMessage = "Failed to update. Try again!";
            }
            return View(updateform);
        }




        [HttpDelete("{id}")]
        [Route("admin/delete-role")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var userWithRole = await _context.UserRoles.Where(x => x.RoleId == id).ToListAsync();
            var role = await _context.Roles.Where(x => x.Name == "unknown").FirstOrDefaultAsync();
            foreach (var userRole in userWithRole)
            {
                var user = await _context.Users.FindAsync(userRole.UserId);
                await _userManager.AddToRoleAsync(user, "unknown");
                _context.SaveChanges();
            }
            var deleteRoleFromIdentity = await _roleManager.Roles.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (deleteRoleFromIdentity != null)
            {
                _context.Roles.Remove(deleteRoleFromIdentity);
                await _context.SaveChangesAsync();
            }
            else
                return RedirectToAction("Index", "NotFound");

            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
