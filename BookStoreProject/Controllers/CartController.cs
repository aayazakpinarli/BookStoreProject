using APP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreProject.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private int GetUserId() => Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == "Id")?.Value);

        public IActionResult Index()
        {
            var cartGroupedBy = _cartService.GetCartGroupedBy(GetUserId());
            return View(cartGroupedBy);
        }

        public IActionResult Clear()
        {
            _cartService.ClearCart(GetUserId());
            TempData["Message"] = "Cart cleared.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int BookId)
        {
            _cartService.RemoveFromCart(GetUserId(), BookId);
            TempData["Message"] = "Book removed from cart.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Add(int BookId)
        {
            Console.WriteLine(BookId);
            Console.WriteLine("HERE ISSS BOOK ID IN CONTROLLER ADD!!");
            _cartService.AddToCart(GetUserId(), BookId);
            TempData["Message"] = "Book added to cart.";
            return RedirectToAction("Index", "");
        }
    }
}