using System.ComponentModel.DataAnnotations;

namespace mvc_app_login.Models.Forms
{
    public class AdminRoleForm
    {
        [Display(Name = "Role name")]
        [Required(ErrorMessage = "A role name is required.")]
        [RegularExpression(@"^[A-Öa-ö]{2,}$", ErrorMessage = "Needs to be a valid first name.")]
        [StringLength(256, ErrorMessage = "Min. 2 characters required.", MinimumLength = 2)]
        public string Name { get; set; } = "";

        public string ErrorMessage { get; set; } = "";
        public string ReturnUrl { get; set; } = "/";
        public string Id { get; set; } = "";
    }
}
