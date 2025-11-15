using APP.Domain;
using CORE.APP.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class UserRequest : Request
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        public string Password { get; set; }

        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public Genders Gender { get; set; }

        [DisplayName("Birth Date")]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Roles")]
        public List<int> RoleIds { get; set; }

        [DisplayName("Country")]
        public int? CountryId { get; set; }

        [DisplayName("City")]
        public int? CityId { get; set; }

        [DisplayName("Books")]
        public List<int> BookIds { get; set; } = new List<int>();

    }
}