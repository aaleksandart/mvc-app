using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mvc_app_login.Models.Entities;

namespace mvc_app_login
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public virtual DbSet<UserProfileEntity> UserProfiles { get; set; }
        public virtual DbSet<ProfilePictureEntity> ProfilePictures { get; set; }
    }
}
