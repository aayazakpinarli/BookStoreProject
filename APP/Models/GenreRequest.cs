using CORE.APP.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class GenreRequest : Request
    {
        [Required, StringLength(200)]

        [DisplayName("Genre Name")]
        public string GenreName { get; set; }

    }
}