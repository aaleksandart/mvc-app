using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc_app_login.Models.Entities;
using mvc_app_login.Models.Forms;
using mvc_app_login.Models.Profiles;
using mvc_app_login.Services;

namespace mvc_app_login.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserProfileService _userProfileService;
        private readonly IWebHostEnvironment _webHost;
        private readonly AppDbContext _context;

        public UserProfileController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IUserProfileService userProfileService, IWebHostEnvironment webHost, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userProfileService = userProfileService;
            _webHost = webHost;
            _context = context;
        }

        #region Get

        [HttpGet("{id}")]
        [Route("userprofile/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var displayUserProfile = await _userProfileService.DisplayProfileAsync(id);
            return View(displayUserProfile);
        }

        #endregion

        #region Update

        [HttpGet]
        [Route("update-userprofile")]
        public IActionResult Update(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var updateforms = new UpdateForm();
                if (returnUrl != null)
                    updateforms.ReturnUrl = returnUrl;

                return View(updateforms);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPut("{id}")]
        [Route("update-userprofile")]
        public async Task<IActionResult> PostUpdate(UpdateForm updateform)
        {
            if (ModelState.IsValid)
            {
                var id = User.FindFirst("UserId").Value;
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

        #endregion

        #region Upload

        [HttpGet]
        [Route("upload-picture")]
        public IActionResult UploadPicture()
        {
            UploadForm uploadform = new UploadForm();
            return View(uploadform);
        }

        [HttpPost]
        [Route("upload-picture")]
        public async Task<IActionResult> UploadPicture(string id, UploadForm uploadform)
        {
            if (ModelState.IsValid)
            {
                var profilePicture = new ProfilePictureEntity 
                { 
                    FileName = $"{id}-{uploadform.Picture.FileName}", 
                    UserId = id 
                };

                var existingPicture = await _context.ProfilePictures.Where(x => x.FileName == profilePicture.FileName).FirstOrDefaultAsync();
                if (existingPicture == null)
                {
                    var usedProfilePicture = await _context.ProfilePictures.Where(x => x.UserId == id).FirstOrDefaultAsync();
                    if(usedProfilePicture != null)
                    {
                        _context.ProfilePictures.Remove(usedProfilePicture);
                        await _context.SaveChangesAsync();
                    }
                    var picturePath = Path.Combine($"{_webHost.WebRootPath}/profile-pictures", profilePicture.FileName);
                    using (FileStream fileStream = new(picturePath, FileMode.Create))
                    {
                        await uploadform.Picture.CopyToAsync(fileStream);
                    }

                    await _context.ProfilePictures.AddAsync(profilePicture);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                uploadform.ErrorMessage = "File with the same name already exists in DB.";
            }
            return View(uploadform);
        }

        #endregion

    }
}
