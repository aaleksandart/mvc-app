using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mvc_app_login.Models.Entities;
using mvc_app_login.Models.Forms;
using mvc_app_login.Models.Profiles;

namespace mvc_app_login.Services
{
    public interface IUserProfileService
    {
        Task<ProfileSuccess> CreateProfileAsync(IdentityUser user, UserProfileModel profilemodel);
        Task<UserProfileModel> DisplayProfileAsync(string userId);
        Task<string> GetDisplayName(string email);
        Task<string> GetRoleName(string id);
        Task<string> GetUserId(string userEmail);
        Task<string> GetPicture(string id);
        Task<ProfileSuccess> UpdateProfileAsync(string id, UpdateProfileModel profilemodel);
        Task<ProfileSuccess> DeleteProfileAsync(string id);
    }
    public class UserProfileService : IUserProfileService
    {
        private readonly AppDbContext _context;

        public UserProfileService(AppDbContext context)
        {
            _context = context;
        }

        #region Create

        public async Task<ProfileSuccess> CreateProfileAsync(IdentityUser user, UserProfileModel profilemodel)
        {
            if(await _context.Users.FindAsync(user.Id) != null)
            {
                var userProfile = new UserProfileEntity
                {
                    UserFromIdentityId = user.Id,
                    FirstName = profilemodel.FirstName,
                    LastName = profilemodel.LastName,
                    StreetName = profilemodel.StreetName,
                    PostalCode = profilemodel.PostalCode,
                    City = profilemodel.City
                };

                _context.UserProfiles.Add(userProfile);
                await _context.SaveChangesAsync();
                return new ProfileSuccess {Success = true};
            }
            return new ProfileSuccess { Success = false };
        }

        #endregion

        #region Display

        public async Task<UserProfileModel> DisplayProfileAsync(string userId)
        {
            UserProfileModel newUserProfile = new UserProfileModel();
            var userProfile = await _context.UserProfiles
                .Include(x => x.UserFromIdentity)
                .Where(x => x.UserFromIdentityId == userId)
                .FirstOrDefaultAsync();

            if (userProfile != null)
            {
                newUserProfile = new(
                userProfile.Id,
                userProfile.FirstName,
                userProfile.LastName,
                userProfile.UserFromIdentity.Email,
                userProfile.StreetName,
                userProfile.PostalCode,
                userProfile.City);
            }

            return newUserProfile;
        }

        #endregion

        #region Get
        public async Task<string> GetDisplayName(string id)
        {
            var user = await _context.UserProfiles.Include(x => x.UserFromIdentity).Where(x => x.UserFromIdentityId == id).FirstOrDefaultAsync();
            return $"{user.FirstName} {user.LastName}";
        }

        public async Task<string> GetRoleName(string id)
        {
            var userRole = await _context.UserRoles.Where(x => x.UserId == id).FirstOrDefaultAsync();
            var role = await _context.Roles.Where(x => x.Id == userRole.RoleId).FirstOrDefaultAsync();
            if (role != null)
                return role.Name.ToUpper();

            return "UNKNOWN";
        }

        public async Task<string> GetUserId(string userEmail)
        {
            var userId = await _context.UserProfiles.Include(x => x.UserFromIdentity).Where(x => x.UserFromIdentity.Email == userEmail).FirstOrDefaultAsync();
            if(userId != null)
                return userId.UserFromIdentity.Id;
            
            return "UNKNOWN";
        }

        public async Task<string> GetPicture(string id)
        {
            var picture = await _context.ProfilePictures.Where(x => x.UserId == id).FirstOrDefaultAsync();
            var filename = "";
            if (picture != null)
                filename = picture.FileName;

            return filename;
        }

        #endregion

        #region Update&Delete
        public async Task<ProfileSuccess> UpdateProfileAsync(string id, UpdateProfileModel updatedProfilemodel)
        {
            
            var userProfileEntity = await _context.UserProfiles.Include(x => x.UserFromIdentity)
            .Where(x => x.UserFromIdentity.Id == id)
            .FirstOrDefaultAsync();

            if (userProfileEntity != null)
            {
                if (!string.IsNullOrEmpty(updatedProfilemodel.FirstName))
                    userProfileEntity.FirstName = updatedProfilemodel.FirstName;
                if (!string.IsNullOrEmpty(updatedProfilemodel.LastName))
                    userProfileEntity.LastName = updatedProfilemodel.LastName;
                if (!string.IsNullOrEmpty(updatedProfilemodel.StreetName))
                    userProfileEntity.StreetName = updatedProfilemodel.StreetName;
                if (!string.IsNullOrEmpty(updatedProfilemodel.PostalCode))
                    userProfileEntity.PostalCode = updatedProfilemodel.PostalCode;
                if (!string.IsNullOrEmpty(updatedProfilemodel.City))
                    userProfileEntity.City = updatedProfilemodel.City;

                _context.Entry(userProfileEntity).State = EntityState.Modified;
                _context.SaveChanges();
                return new ProfileSuccess { Success = true };
            }

            return new ProfileSuccess { Success = false };
        }
        public async Task<ProfileSuccess> DeleteProfileAsync(string id)
        {
            var deleteProfile = await _context.UserProfiles.Include(x => x.UserFromIdentity).Where(x => x.UserFromIdentityId == id).FirstOrDefaultAsync();
            if (deleteProfile != null)
            {
                _context.UserProfiles.Remove(deleteProfile);
                await _context.SaveChangesAsync();
                return new ProfileSuccess { Success = true };
            }
            
            return new ProfileSuccess { Success = false };
        }

        #endregion
    }
    public class ProfileSuccess
    {
        public bool Success { get; set; } = false;
    }
}
