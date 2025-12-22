using System.ComponentModel;

namespace APP.Models
{
    public class CartItem
    {
        public int UserId { get; set; }

        public int BookId { get; set; }

        [DisplayName("Book Name")]
        public string BookName { get; set; }

        public decimal UnitPrice { get; set; }

        [DisplayName("Unit Price")]
        public string UnitPriceF { get; set; }
    }
}