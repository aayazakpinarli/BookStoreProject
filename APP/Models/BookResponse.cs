using APP.Domain;
using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    public class BookResponse : Response
    {

        public string BookName { get; set; }

        public string Description { get; set; }

        public short? NumberOfPages { get; set; }

        public DateTime PublishedOn { get; set; }

        public decimal Price { get; set; }

        public bool IsTopSeller { get; set; }

        public int AuthorId { get; set; }

        public int? StockAmount { get; set; }

        public List<int> GenreIds { get; set; }


        // Formatted Values

        [DisplayName("Book Price")]
        public string PriceF { get; set; }

        [DisplayName("Stock Amount")]
        public string StockAmountF { get; set; }


        public List<string> Authors { get; set; } 

        public List<string> BookGenres { get; set; }

    }
}