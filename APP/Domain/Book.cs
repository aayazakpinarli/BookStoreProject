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

        public List<Author> Authors { get; set; } = new List<Author>();

        public List<BookGenre> BookGenres { get; set; } = new List<BookGenre>();

        [NotMapped]
        public List<int> GenreIds
        {
            get => BookGenres.Select(bookGenreEntity => bookGenreEntity.GenreId).ToList();
            set => BookGenres = value?.Select(genreId => new BookGenre() { GenreId = genreId }).ToList();
        }


        

    }
}