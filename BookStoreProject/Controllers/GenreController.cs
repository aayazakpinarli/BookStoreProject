#nullable disable
using APP.Models;
using APP.Services;
using CORE.APP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStoreProject.Controllers
{
    public class GenreController : Controller
    {
        private readonly IService<GenreRequest, GenreResponse> _genreService;

        public GenreController(
            IService<GenreRequest, GenreResponse> GenreService
        )
        {
            _genreService = GenreService;
        }

        private void SetViewData()
        {
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        public IActionResult Index()
        {
            var list = _genreService.List();
            return View(list); 
        }

        public IActionResult Details(int id)
        {
            var item = _genreService.Item(id);
            return View(item);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            SetViewData();
            return View(); 
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(GenreRequest genre)
        {
            if (ModelState.IsValid) 
            {
                var response = _genreService.Create(genre);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Index), new { id = response.Id }); 
                }
                ModelState.AddModelError("", response.Message); 
            }
            SetViewData(); 
            return View(genre); 
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var item = _genreService.Edit(id);
            SetViewData(); 
            return View(item); 
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(GenreRequest genre)
        {
            if (ModelState.IsValid) 
            {
                var response = _genreService.Update(genre);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); 
                    return RedirectToAction(nameof(Details), new { id = response.Id }); 
                }
                ModelState.AddModelError("", response.Message); 
            }
            SetViewData(); 
            return View(genre); 
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var item = _genreService.Item(id);
            return View(item); 
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _genreService.Delete(id);
            SetTempData(response.Message); 
            return RedirectToAction(nameof(Index)); 
        }
    }
}