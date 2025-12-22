using CORE.APP.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class AuthorRequest : Request
    {
        [Required]
        [StringLength(30)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }


        [DisplayName("Last Name")]
        public string LastName { get; set; } 
    }
}