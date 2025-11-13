using APP.Models;
using APP.Services;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{

    // [Obsolete("Use BookController class instead!")]
    public class BookObsoleteController : Controller
    {
        // The service that provides business logic of CRUD operations for authors.
        private readonly BookObsoleteService _authorService;


        public BookObsoleteController(BookObsoleteService AuthorService)
        {
            _authorService = AuthorService;
        }


        public IActionResult Index()
        {
            var list = _authorService.Query().ToList();

            ViewBag.Count = list.Count == 0 ? "No Authors found!" : list.Count == 1 ? "1 Author found." : $"{list.Count} Authors found.";

            return View(list);
        }

        public IActionResult Details(int id)
        {
            var item = _authorService.Query().SingleOrDefault(authorResponse => authorResponse.Id == id);

            if (item is null)
                ViewBag.Message = "Author not found!";

            return View(item);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost] 
        public IActionResult Create(AuthorRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = _authorService.Create(request);

                if (response.IsSuccessful)
                {
                    TempData["Message"] = response.Message;

                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Message = response.Message;
            }

            return View(request);
        }

        public IActionResult Edit(int id)
        {
            var request = _authorService.Edit(id);

            if (request is null)
                ViewBag.Message = "Author not found!";

            return View(request);
        }

        [HttpPost] 
        public IActionResult Edit(AuthorRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = _authorService.Update(request);

                if (response.IsSuccessful)
                    return RedirectToAction(nameof(Details), new { id = response.Id });

                ViewBag.Message = response.Message;
            }

            return View(request);
        }

        public IActionResult Delete(int id)
        {
            var response = _authorService.Delete(id);

            TempData["Message"] = response.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}