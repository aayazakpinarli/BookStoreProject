using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    public class AuthorResponse : Response
    {
        [DisplayName("Author Name")]
        public string FirstName { get; set; }

        [DisplayName("Author Lastname")]
        public string LastName { get; set; }

        [DisplayName("Author")]
        public string FullName { get; internal set; }

        [DisplayName("Book Count")]
        public int BookCount { get; set; }

        [DisplayName("Author's Book")]
        public List<string> Books { get; set; }
    }
}