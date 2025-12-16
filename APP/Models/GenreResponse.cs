using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    public class GenreResponse : Response
    {

        [DisplayName("Genre Name")]
        public string GenreName { get; set; }

        [DisplayName("Book Count")]
        public int BookCount { get; set; }
        public string Books { get; set; }
    }
}