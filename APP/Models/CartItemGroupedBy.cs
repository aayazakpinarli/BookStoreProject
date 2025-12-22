using System.ComponentModel;

namespace APP.Models
{
    public class CartItemGroupedBy
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        [DisplayName("Book Name")]
        public string BookName { get; set; }
        [DisplayName("Book Count")]
        public int BookCount { get; set; }
        public decimal TotalPrice { get; set; }
        [DisplayName("Total Price")]
        public string TotalPriceF { get; set; }
    }
}