using CORE.APP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace APP.Models
{
    public class BookQueryRequest : Request
    {
        [DisplayName("Book Name")]
        public string BookName { get; set; }

        [DisplayName("Authors")]
        public List<int> AuthorIds { get; set; } = new List<int>();

        [DisplayName("Genres")]
        public List<int> GenreIds { get; set; } = new List<int>();

        [DisplayName("Minimum Price")]
        public decimal? MinPrice { get; set; }

        [DisplayName("Maximum Price")]
        public decimal? MaxPrice { get; set; }

        [DisplayName("Published From")]
        public DateTime? PublishedFrom { get; set; }

        [DisplayName("Published To")]
        public DateTime? PublishedTo { get; set; }

        [DisplayName("Top Seller")]
        public bool? IsTopSeller { get; set; }

        [DisplayName("Has Stock")]
        public bool? HasStock { get; set; }
    }
}
