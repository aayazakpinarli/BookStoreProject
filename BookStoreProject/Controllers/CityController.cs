#nullable disable
using APP.Models;
using APP.Services;
using CORE.APP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStoreProject.Controllers
{
    public class CityController : Controller
    {
        private readonly IService<CityRequest, CityResponse> _cityService;
        private readonly IService<CountryRequest, CountryResponse> _countryService;

        public CityController(
            IService<CityRequest, CityResponse> cityService,
            IService<CountryRequest, CountryResponse> countryService
        )
        {
            _cityService = cityService;
            _countryService = countryService;
        }

        private void SetViewData()
        {
            ViewData["CountryId"] = new SelectList(_countryService.List(), "Id", "CountryName");
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        public IActionResult Index()
        {
            var list = _cityService.List();
            return View(list);
        }

        public IActionResult Details(int id)
        {
            var item = _cityService.Item(id);
            return View(item);
        }

        public IActionResult Create()
        {
            SetViewData();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CityRequest city)
        {
            if (ModelState.IsValid)
            {
                var response = _cityService.Create(city);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(city);
        }

        public IActionResult Edit(int id)
        {
            var item = _cityService.Edit(id);
            SetViewData();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(CityRequest city)
        {
            if (ModelState.IsValid)
            {
                var response = _cityService.Update(city);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(city);
        }

        public IActionResult Delete(int id)
        {
            var item = _cityService.Item(id);
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _cityService.Delete(id);
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Json(int? countryId)
        {
            var cityService = _cityService as CityService;
            var list = cityService.List(countryId);
            return Json(list);
        }

        public IActionResult DeleteFile(int id, string filePath)
        {
            var cityService = _cityService as CityService;
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
