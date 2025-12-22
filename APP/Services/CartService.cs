using APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.Session.MVC;
using Microsoft.Extensions.Logging.Console;

namespace APP.Services
{
    public class CartService : ICartService
    {
        const string SESSIONKEY = "cart";

        private readonly SessionServiceBase _sessionService;

        private readonly IService<BookRequest, BookResponse> _bookService;

        public CartService(SessionServiceBase sessionService, IService<BookRequest, BookResponse> bookService)
        {
            _bookService = bookService;
            _sessionService = sessionService;
        }

        public List<CartItem> GetCart(int userId)
        {
            var cart = _sessionService.GetSession<List<CartItem>>(SESSIONKEY);
            if (cart is not null)
                return cart.Where(c => c.UserId == userId).ToList();
            return new List<CartItem>();
        }

        public List<CartItemGroupedBy> GetCartGroupedBy(int userId)
        {
            var cart = GetCart(userId);

            return cart
                .GroupBy(cartItem => new // group cart items
                {
                    cartItem.UserId,
                    cartItem.BookId,
                    cartItem.BookName
                })
                .Select(cartItemGroupedBy => new CartItemGroupedBy
                {
                    UserId = cartItemGroupedBy.Key.UserId, 
                    BookId = cartItemGroupedBy.Key.BookId, 
                    BookName = cartItemGroupedBy.Key.BookName, 
                    BookCount = cartItemGroupedBy.Count(),
                    TotalPrice = cartItemGroupedBy.Sum(cartItem => cartItem.UnitPrice),
                    TotalPriceF = cartItemGroupedBy.Sum(cartItem => cartItem.UnitPrice).ToString("C2") 
                }).ToList();
        }

        public void AddToCart(int userId, int bookId)
        {
            Console.WriteLine("Here is my first console write");
            Console.WriteLine(bookId);
            Console.WriteLine("Here is after bookid");

            Console.WriteLine(userId);

            Console.WriteLine("Here is after user id");


            var book = _bookService.Item(bookId);

            if (book is not null)
            {
                var cart = GetCart(userId);
                cart.Add(new CartItem
                {
                    UserId = userId,
                    BookId = book.Id,
                    BookName = book.BookName,
                    UnitPrice = book.Price,
                    UnitPriceF = book.PriceF
                });
                _sessionService.SetSession(SESSIONKEY, cart);
            }
        }

        public void RemoveFromCart(int userId, int bookId)
        {
            var cart = GetCart(userId);
            var cartItem = cart.FirstOrDefault(c => c.UserId == userId && c.BookId == bookId);
            if (cartItem is not null)
                cart.Remove(cartItem);
            _sessionService.SetSession(SESSIONKEY, cart);
        }
        public void ClearCart(int userId)
        {
            var cart = GetCart(userId);
            cart.RemoveAll(c => c.UserId == userId);
            _sessionService.SetSession(SESSIONKEY, cart);
        }
    }
}