#nullable disable
using APP.Models;
using APP.Services;
using CORE.APP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStoreProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService<GenreRequest, GenreResponse> _genreService;
        private readonly IService<BookRequest, BookResponse> _bookService;
        private readonly IService<AuthorRequest, AuthorResponse> _authorService;


        public HomeController(
            IService<BookRequest, BookResponse> bookService,
            IService<AuthorRequest, AuthorResponse> authorService,
            IService<GenreRequest, GenreResponse> genreService)
        {
            _bookService = bookService;
            _authorService = authorService;
            _genreService = genreService;
        }

        private void SetViewData()
        {
            ViewData["BookIds"] = new MultiSelectList(_bookService.List(), "Id", "BookName", "IsTopSeller", "PriceF");
            ViewData["AuthorIds"] = new MultiSelectList(_authorService.List(), "Id", "FullName");
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        [HttpGet]
        public IActionResult Index(HomeRequest request)
        {

            // guards
            if (request is null) request = new HomeRequest();
            if (request.Page < 1) request.Page = 1;
            if (request.PageSize < 1) request.PageSize = 12;
            if (string.IsNullOrWhiteSpace(request.SelectedCategory)) request.SelectedCategory = "All";
            if (string.IsNullOrWhiteSpace(request.Sort)) request.Sort = "newest";

            var books = _bookService.List();

            // search: BookName, Description, Authors
            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var q = request.Query.Trim();

                books = books.Where(b =>
                    (!string.IsNullOrWhiteSpace(b.BookName) &&
                     b.BookName.Contains(q, StringComparison.OrdinalIgnoreCase))
                    ||
                    (!string.IsNullOrWhiteSpace(b.Description) &&
                     b.Description.Contains(q, StringComparison.OrdinalIgnoreCase))
                    ||
                    (b.Authors != null && b.Authors.Any(a =>
                        !string.IsNullOrWhiteSpace(a) &&
                        a.Contains(q, StringComparison.OrdinalIgnoreCase)))
                ).ToList();
            }

            // category filter: use BookGenres (list of strings)
            if (!string.IsNullOrWhiteSpace(request.SelectedCategory) && request.SelectedCategory != "All")
            {
                var cat = request.SelectedCategory.Trim();

                books = books.Where(b =>
                    b.BookGenres != null && b.BookGenres.Any(g =>
                        !string.IsNullOrWhiteSpace(g) &&
                        string.Equals(g, cat, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            // sort: newest, price, title
            books = request.Sort switch
            {
                "price_asc" => books.OrderBy(b => b.Price).ToList(),
                "price_desc" => books.OrderByDescending(b => b.Price).ToList(),
                "title_asc" => books.OrderBy(b => b.BookName).ToList(),
                _ => books.OrderByDescending(b => b.PublishedOn).ToList() // newest
            };

            var totalCount = books.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

            // build HomeResponse (matches your class)
            var response = new HomeResponse
            {
                Query = request.Query,
                SelectedCategory = request.SelectedCategory,
                Sort = request.Sort,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,

                // optional single values (your HomeResponse has these)
                Title = request.Query ?? "",
                Author = "",

                Genres = books
                    .SelectMany(b => b.BookGenres ?? new List<string>())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(x => x)
                    .ToList(),

                Authors = books
                    .SelectMany(b => b.Authors ?? new List<string>())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(x => x)
                    .ToList(),

                Books = books
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList()
            };

            return View(response);
        
        }

        public IActionResult Details(int id)
        {
            var item = _bookService.Item(id);
            return View(item);
        }

        [HttpGet]
        public IActionResult Search(string q)
        {
            var list = _bookService.List();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                list = list.Where(b =>
                    (!string.IsNullOrEmpty(b.BookName) && b.BookName.Contains(q, StringComparison.OrdinalIgnoreCase)) || 
                    (b.Authors != null && b.Authors.Any(a => !string.IsNullOrWhiteSpace(a) &&
                        a.Contains(q, StringComparison.OrdinalIgnoreCase)
                    ))
                ).ToList();
            }

            ViewBag.Query = q;
            return View("Index", list);
        }
    }
}
