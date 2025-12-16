using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must be the same!")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}