#nullable disable
using Microsoft.AspNetCore.Mvc;
using CORE.APP.Services;
using APP.Models;

namespace MVC.Controllers
{
    public class CountryController : Controller
    {
        private readonly IService<CountryRequest, CountryResponse> _countryService;


        public CountryController(
            IService<CountryRequest, CountryResponse> countryService
        )
        {
            _countryService = countryService;
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
            var list = _countryService.List();
            return View(list); 
        }

        public IActionResult Details(int id)
        {
            var item = _countryService.Item(id);
            return View(item); 
        }

        public IActionResult Create()
        {
            SetViewData();
            return View(); 
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CountryRequest country)
        {
            if (ModelState.IsValid) 
            {
                var response = _countryService.Create(country);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id }); 
                }
                ModelState.AddModelError("", response.Message); 
            }
            SetViewData();
            return View(country); 
        }

        public IActionResult Edit(int id)
        {
            var item = _countryService.Edit(id);
            SetViewData();
            return View(item); 
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(CountryRequest country)
        {
            if (ModelState.IsValid) 
            {
                var response = _countryService.Update(country);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(country); 
        }

        public IActionResult Delete(int id)
        {
            var item = _countryService.Item(id);
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _countryService.Delete(id);
            SetTempData(response.Message); 
            return RedirectToAction(nameof(Index)); 
        }
    }
}