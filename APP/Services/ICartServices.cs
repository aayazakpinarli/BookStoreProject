using APP.Models;

namespace APP.Services
{
    public interface ICartService
    {
        public List<CartItem> GetCart(int userId); // public in default 

        public List<CartItemGroupedBy> GetCartGroupedBy(int userId);

        public void AddToCart(int userId, int bookId);

        public void RemoveFromCart(int userId, int bookId);

        public void ClearCart(int userId);
    }
}