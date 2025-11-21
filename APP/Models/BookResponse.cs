using APP.Domain;
using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    public class BookResponse : Response
    {

        [DisplayName("Book Name")]
        public string BookName { get; set; }


        [DisplayName("Description")]
        public string Description { get; set; }


        [DisplayName("Number of Pages")]
        public short? NumberOfPages { get; set; }


        [DisplayName("Publish Date")]
        public DateTime PublishedOn { get; set; }


        [DisplayName("Price")]
        public decimal Price { get; set; }


        [DisplayName("Top Seller")]
        public bool IsTopSeller { get; set; }

        public int AuthorId { get; set; }


        [DisplayName("Stock Amount")]
        public int? StockAmount { get; set; }

        public List<int> GenreIds { get; set; }


        // Formatted Values

        [DisplayName("Book Price")]
        public string PriceF { get; set; }

        [DisplayName("Stock Amount")]
        public string StockAmountF { get; set; }


        public List<string> Authors { get; set; }


        [DisplayName("Book Genres")] 
        public List<string> BookGenres { get; set; }

    }
}