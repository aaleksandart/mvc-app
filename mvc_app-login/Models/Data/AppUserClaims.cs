using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace mvc_app_login.Models.Data
{
    public class AppUserClaims : UserClaimsPrincipalFactory<IdentityUser, IdentityRole>
    {
        public AppUserClaims(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IdentityUser user)
        {
            var claimsFromIdentity = await base.GenerateClaimsAsync(user);
            claimsFromIdentity.AddClaim(new Claim("UserId", user.Id));

            return claimsFromIdentity;
        }
    }
}
