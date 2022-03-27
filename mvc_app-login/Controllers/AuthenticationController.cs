using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc_app_login.Models.Forms;
using mvc_app_login.Models.Profiles;
using mvc_app_login.Services;

namespace mvc_app_login.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserProfileService _userProfileService;

        public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IUserProfileService userProfileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userProfileService = userProfileService;
        }

        #region Register

        [HttpGet]
        [Route("register")]
        public IActionResult Register(string returnUrl = null)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                var registerforms = new RegisterForm();
                if (returnUrl != null)
                    registerforms.ReturnUrl = returnUrl;

                return View(registerforms);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterForm registerform)
        {
            if (ModelState.IsValid)
            {
                var users = await _userManager.Users.AnyAsync();
                var admins = await _roleManager.Roles.AnyAsync();
                var userAdmin = await _userManager.GetUsersInRoleAsync("admin");

                if (!admins)
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("user"));
                    await _roleManager.CreateAsync(new IdentityRole("unknown"));
                }

                if (!users || userAdmin.Count == 0 || userAdmin == null)
                    registerform.RoleName = "admin";
                else
                {
                    if (!await _roleManager.RoleExistsAsync("user"))
                        await _roleManager.CreateAsync(new IdentityRole("user"));

                    registerform.RoleName = "user";
                }

                if (!await _userManager.Users.Where(x => x.Email == registerform.Email).AnyAsync())
                {
                    var user = new IdentityUser()
                    {
                        UserName = registerform.Email,
                        Email = registerform.Email
                    };

                    var regUser = await _userManager.CreateAsync(user, registerform.Password);
                    if (regUser.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, registerform.RoleName);
                        var userProfile = new UserProfileModel
                        {
                            FirstName = registerform.FirstName,
                            LastName = registerform.LastName,
                            Email = user.Email,
                            StreetName = registerform.StreetName,
                            PostalCode = registerform.PostalCode,
                            City = registerform.City
                        };

                        var regUserProfile = await _userProfileService.CreateProfileAsync(user, userProfile);
                        if (regUserProfile.Success)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);

                            if (registerform.ReturnUrl == null || registerform.ReturnUrl == "/")
                                return RedirectToAction("Index", "Home");
                            else
                                return LocalRedirect(registerform.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "NotFound");
                        }
                    }
                }
                else
                {
                    registerform.ErrorMessage = "The email you tried to register\nalready exists in the database.";
                }
            }

            return View(registerform);
        }

        #endregion


        #region Login

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl = null)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                var loginforms = new LoginForm();
                if (returnUrl != null)
                    loginforms.ReturnUrl = returnUrl;

                return View(loginforms);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginForm loginform)
        {
            if (ModelState.IsValid)
            {
                var login_success = await _signInManager.PasswordSignInAsync(loginform.Email, loginform.Password, isPersistent: false, false);
                if (login_success.Succeeded)
                {
                    if (loginform.ReturnUrl == null || loginform.ReturnUrl == "/")
                        return RedirectToAction("Index", "Home");
                    else
                        return LocalRedirect(loginform.ReturnUrl);
                }
            }
            loginform.ErrorMessage = "Email or password is not correct!";
            return View(loginform);
        }

        #endregion


        #region Logout

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            if (_signInManager.IsSignedIn(User))
                await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
