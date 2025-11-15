#nullable disable
using APP.Models;
using APP.Services;
using CORE.APP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace BookStoreProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IService<UserRequest, UserResponse> _userService;
        private readonly IService<CityRequest, CityResponse> _cityService;
        private readonly IService<CountryRequest, CountryResponse> _countryService;
        private readonly IService<RoleRequest, RoleResponse> _RoleService;

        public UserController(
            IService<UserRequest, UserResponse> userService,
            IService<CityRequest, CityResponse> cityService,
            IService<CountryRequest, CountryResponse> countryService,
            IService<RoleRequest, RoleResponse> RoleService
        )
        {
            _userService = userService;
            _cityService = cityService;
            _countryService = countryService;
            _RoleService = RoleService;
        }

        private void SetViewData()
        {
            ViewBag.RoleIds = new MultiSelectList(_RoleService.List(), "Id", "Name");
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        public IActionResult Index()
        {
            Trace.WriteLine("Hello");
            Debug.WriteLine("This is a log");
            var list = _userService.List();
            return View(list);
        }

        public IActionResult Details(int id)
        {
            var item = _userService.Item(id);
            return View(item);
        }

        public IActionResult Create()
        {
            SetViewData();
            ViewData["CountryId"] = new SelectList(_countryService.List(), "Id", "CountryName");
            ViewData["CityId"] = new SelectList(_cityService.List(), "Id", "CityName");
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserRequest user)
        {
            if (ModelState.IsValid)
            {
                var response = _userService.Create(user);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }

            SetViewData();
            ViewData["CountryId"] = new SelectList(_countryService.List(), "Id", "CountryName", user.CountryId);
            ViewData["CityId"] = new SelectList(_cityService.List(), "Id", "CityName", user.CityId);

            return View(user);
        }

        public IActionResult Edit(int id)
        {
            var item = _userService.Edit(id);

            SetViewData();
            ViewData["CountryId"] = new SelectList(_countryService.List(), "Id", "CountryName", item.CountryId);

            var cityService = _cityService as CityService;
            ViewData["CityId"] = new SelectList(cityService.List(item.CountryId), "Id", "CityName", item.CityId);

            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(UserRequest user)
        {
            if (ModelState.IsValid)
            {
                var response = _userService.Update(user);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }

            SetViewData();
            ViewData["CountryId"] = new SelectList(_countryService.List(), "Id", "CountryName", user.CountryId);

            var cityService = _cityService as CityService;
            ViewData["CityId"] = new SelectList(cityService.List(user.CountryId), "Id", "CityName", user.CityId);

            return View(user);
        }

        public IActionResult Delete(int id)
        {
            var item = _userService.Item(id);
            return View(item);
        }
        

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _userService.Delete(id);
            SetTempData(response.Message);

            return RedirectToAction(nameof(Index));
        }
    }
}
