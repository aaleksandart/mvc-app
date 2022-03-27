using System.ComponentModel.DataAnnotations;

namespace mvc_app_login.Models.Forms
{
    public class UploadForm
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile Picture { get; set; }

        public string ErrorMessage { get; set; } = "";
    }
}
