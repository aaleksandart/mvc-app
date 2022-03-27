using System.ComponentModel.DataAnnotations;

namespace mvc_app_login.Models.Forms
{
    public class LoginForm
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "An email adress is required.")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$", ErrorMessage = "Needs to be a valid email.")]
        public string Email { get; set; } = "";

        [Display(Name = "Password")]
        [Required(ErrorMessage = "A password is required.")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Needs to be a valid password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        public string ReturnUrl { get; set; } = "/";
        public string ErrorMessage { get; set; } = "";
    }
}
