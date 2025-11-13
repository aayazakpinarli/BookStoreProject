using CORE.APP.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class GenreRequest : Request
    {
        [Required, StringLength(200)]
        public string GenreName { get; set; }

    }
}