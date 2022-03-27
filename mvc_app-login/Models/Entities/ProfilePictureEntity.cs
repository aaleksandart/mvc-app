using System.ComponentModel.DataAnnotations;

namespace mvc_app_login.Models.Entities
{
    public class ProfilePictureEntity
    {
        [Key]
        public string FileName { get; set; }

        [Required]
        public string UserId { get; set; } 
    }
}
