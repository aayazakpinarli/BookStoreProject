using APP.Models;
using APP.Services;
using CORE.APP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStoreProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorsController : Controller
    {

        private readonly IService<AuthorRequest, AuthorResponse> _authorService;

        public AuthorsController(
            IService<AuthorRequest, AuthorResponse> AuthorService)
        {
            _authorService = AuthorService;
        }


        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        public IActionResult Index()
        {
            var list = _authorService.List();
            return View(list);
        }

        public IActionResult Details(int id)
        {
            var item = _authorService.Item(id);

            if (item is null)
            {
                ViewBag.Message = "Author not found!";
                return RedirectToAction(nameof(Index));
            }

            return View(item);
        }


        [HttpGet]

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var request = _authorService.Edit(id);

            if (request is null)
                ViewBag.Message = "Author not found!";

            return View(request);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var item = _authorService.Item(id);
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _authorService.Delete(id);
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}