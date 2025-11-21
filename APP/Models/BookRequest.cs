using CORE.APP.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class BookRequest : Request
    {

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        [DisplayName("Book Name")]
        public string BookName { get; set; }

        [DisplayName("Book Description")]
        public string Description { get; set; }

        [DisplayName("Number Of Pages")]
        public short? NumberOfPages { get; set; }


        [DisplayName("Published On")]
        public DateTime PublishedOn { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "{0} must be a positive decimal number!")] 
        [DisplayName("Book Price")]
        public decimal BookPrice { get; set; }

        [DisplayName("Top Seller")]
        public bool IsTopSeller { get; set; }

        [Range(0, 100000, ErrorMessage = "{0} must be between {1} and {2}!")] 
        [DisplayName("Stock Amount")]
        public int? StockAmount { get; set; }

        [DisplayName("Authors")]
        [Required]
        public List<int> AuthorIds { get; set; } = new List<int>();

        [DisplayName("Book Genres")]
        public List<int> GenreIds { get; set; } = new List<int>();

    }
}