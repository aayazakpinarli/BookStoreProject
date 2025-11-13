using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    public class GenreResponse : Response
    {
        public string GenreName { get; set; }

        [DisplayName("Book Count")]
        public int GenreCount { get; set; }
        public string Books { get; set; }
    }
}