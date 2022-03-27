using System.ComponentModel.DataAnnotations;

namespace mvc_app_login.Models.Forms
{
    public class RegisterForm
    {
        [Display(Name = "First name")]
        [Required(ErrorMessage = "A first name is required.")]
        [RegularExpression(@"^[A-Öa-ö]{2,}$", ErrorMessage = "Needs to be a valid first name.")]
        [StringLength(256, ErrorMessage = "Min. 2 characters required.", MinimumLength = 2)]
        public string FirstName { get; set; } = "";

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "A last name is required.")]
        [RegularExpression(@"^[A-Öa-ö]{2,}$", ErrorMessage = "Needs to be a valid last name.")]
        [StringLength(256, ErrorMessage = "Min. 2 characters required.", MinimumLength = 2)]
        public string LastName { get; set; } = "";

        [Display(Name = "Password")]
        [Required(ErrorMessage = "A password is required.")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Needs to be a valid password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Password needs to be confirmed.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Not a match to 'Password'")]
        public string ConfirmPassword { get; set; } = "";

        [Display(Name = "Email")]
        [Required(ErrorMessage = "An email adress is required.")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$", ErrorMessage = "Needs to be a valid email.")]
        public string Email { get; set; } = "";

        [Display(Name = "Streetname")]
        [Required(ErrorMessage = "A streetname is required.")]
        [RegularExpression(@"^[A-Öa-ö]{2,}\s*\d*$", ErrorMessage = "Needs to be a valid streetname.")]
        [StringLength(256, ErrorMessage = "Min. 2 characters required.", MinimumLength = 2)]
        public string StreetName { get; set; } = "";

        [Display(Name = "PostalCode")]
        [Required(ErrorMessage = "A postal code needs to be 5 digits long.")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Needs to be a valid postal code.")]
        [StringLength(256, ErrorMessage = "Exact 5 digits required.", MinimumLength = 5)]
        public string PostalCode { get; set; } = "";


        [Display(Name = "City")]
        [Required(ErrorMessage = "A city is required.")]
        [RegularExpression(@"^[A-Öa-ö]{2,}$", ErrorMessage = "Needs to be a valid city.")]
        [StringLength(256, ErrorMessage = "Min. 2 characters required.", MinimumLength = 2)]
        public string City { get; set; } = "";


        public string RoleName { get; set; } = "user";
        public string ErrorMessage { get; set; } = "";
        public string ReturnUrl { get; set; } = "/";


    }
}
