#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CORE.APP.Services;
using APP.Models;

namespace BookStoreProject.Controllers
{
    public class BookController : Controller
    {
        private readonly IService<BookRequest, BookResponse> _bookService;
        private readonly IService<AuthorRequest, AuthorResponse> _authorService;
        private readonly IService<GenreRequest, GenreResponse> _genreService;

        public BookController(
            IService<BookRequest, BookResponse> bookService,
            IService<AuthorRequest, AuthorResponse> authorService,
            IService<GenreRequest, GenreResponse> genreService
        )
        {
            _bookService = bookService;
            _authorService = authorService;
            _genreService = genreService;
        }


        private void SetViewData()
        {
            ViewData["AuthorIds"] = new MultiSelectList(_authorService.List(), "Id", "FullName");
            ViewData["GenreIds"] = new MultiSelectList(_genreService.List(), "Id", "GenreName");
        }


        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }


        public IActionResult Index()
        {
            var list = _bookService.List();
            return View(list);
        }


        public IActionResult Details(int id)
        {
            var item = _bookService.Item(id);
            if (item == null)
            {
                SetTempData("Book not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }


        public IActionResult Create()
        {
            SetViewData();
            return View();
        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(BookRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = _bookService.Create(request);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Index), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(request);
        }


        public IActionResult Edit(int id)
        {
            var request = _bookService.Edit(id);
            if (request == null)
            {
                SetTempData("Book not found!");
                return RedirectToAction(nameof(Index));
            }
            SetViewData();
            return View(request);
        }

        
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(BookRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = _bookService.Update(request);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(request);
        }


        public IActionResult Delete(int id)
        {
            var item = _bookService.Item(id);
            if (item == null)
            {
                SetTempData("Book not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(BookResponse book)
        {
            if (book is null)
                return RedirectToAction(nameof(Index));

            var response = _bookService.Delete(book.Id);
            TempData["Message"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

    }
}
