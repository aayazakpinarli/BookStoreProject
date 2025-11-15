using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Domain
{
    public class Book : Entity
    {
        [Required]
        [StringLength(100)]
        public string BookName { get; set; }
        public string Description { get; set; }
        public short? NumberOfPages { get; set; }
        public DateTime PublishedOn { get; set; }
        public decimal Price { get; set; }  
        public bool IsTopSeller { get; set; }
        public int AuthorId { get; set; }
        public int? StockAmount { get; set; }

        public List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

        public List<BookGenre> BookGenres { get; set; } = new List<BookGenre>();

        public List<UserBook> UserBooks { get; set; } = new List<UserBook>();

        [NotMapped]
        public List<int> GenreIds
        {
            get => BookGenres.Select(bookGenreEntity => bookGenreEntity.GenreId).ToList();
            set => BookGenres = value?.Select(genreId => new BookGenre() { GenreId = genreId }).ToList();
        }

        [NotMapped]
        public List<int> AuthorIds
        {
            get => BookAuthors.Select(bookAuthorEntity => bookAuthorEntity.AuthorId).ToList();
            set => BookAuthors = value?.Select(authorId => new BookAuthor() { AuthorId = authorId }).ToList();
        }

    }
}