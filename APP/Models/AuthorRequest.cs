using CORE.APP.Models;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class AuthorRequest : Request
    {
        [Required]
        [StringLength(30)]
        public string FirstName { get; set; } 

        public string LastName { get; set; } 
    }
}