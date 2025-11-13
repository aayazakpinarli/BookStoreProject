using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.Domain
{
    public class Genre : Entity
    {
        [Required]
        [StringLength(100)]
        public string GenreName { get; set; }

        // for book-genre many to many relationship
        public List<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
    }
}