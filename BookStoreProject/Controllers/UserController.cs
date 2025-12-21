#nullable disable
using APP.Models;
using APP.Services;
using CORE.APP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace BookStoreProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IService<UserRequest, UserResponse> _userService;
        private readonly IService<CityRequest, CityResponse> _cityService;
        private readonly IService<CountryRequest, CountryResponse> _countryService;
        private readonly IService<RoleRequest, RoleResponse> _RoleService;
        private readonly IService<BookRequest, BookResponse> _bookService;

        public UserController(
            IService<UserRequest, UserResponse> userService,
            IService<CityRequest, CityResponse> cityService,
            IService<CountryRequest, CountryResponse> countryService,
            IService<RoleRequest, RoleResponse> RoleService,
            IService<BookRequest, BookResponse> bookService
        )
        {
            _userService = userService;
            _cityService = cityService;
            _countryService = countryService;
            _RoleService = RoleService;
            _bookService = bookService;
        }

        private void SetViewData()
        {
            ViewData["RoleIds"] = new MultiSelectList(_RoleService.List(), "Id", "Name");
            ViewData["BookIds"] = new MultiSelectList(_bookService.List(), "Id", "BookName");
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        [Authorize]
        public IActionResult Index()
        {
            var list = _userService.List();
            return View(list);
        }

        private bool IsOwnAccount(int id) 
        {
            return id.ToString() == (User.Claims.SingleOrDefault(claim => claim.Type == "Id")?.Value ?? string.Empty);
        }


        [Authorize] // Authenticated users only
        public IActionResult Details(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var item = _userService.Item(id);
            if (item == null)
            {
                SetTempData("User is not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        [Authorize(Roles = "Admin")] // Admins only
        public IActionResult Create()
        {
            SetViewData();
            ViewData["CountryId"] = new SelectList(_countryService.List(), "Id", "CountryName");
            ViewData["CityId"] = new SelectList(_cityService.List(), "Id", "CityName");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

        [Authorize]
        public IActionResult Edit(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var item = _userService.Edit(id);
            if (item == null)
            {
                SetTempData("User is not found!");
                return RedirectToAction(nameof(Index));
            }

            SetViewData();
            ViewData["CountryId"] = new SelectList(_countryService.List(), "Id", "CountryName", item.CountryId);

            var cityService = _cityService as CityService;
            ViewData["CityId"] = new SelectList(cityService.List(item.CountryId), "Id", "CityName", item.CityId);

            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(UserRequest user)
        {
            if (!IsOwnAccount(user.Id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            if (!User.IsInRole("Admin"))
            {
                ModelState.Remove(nameof(UserRequest.RoleIds));
            }

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
        
        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var item = _userService.Item(id);
            if (item == null)
            {
                SetTempData("User is not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }
        

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        [Authorize]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var response = _userService.Delete(id);
            SetTempData(response.Message);

            // if the user deleted his/her own account, log out the user
            if (IsOwnAccount(id))
                return RedirectToAction(nameof(Logout));

            return RedirectToAction(nameof(Index));
        }

        [Route("~/[action]")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("~/[action]")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            if (ModelState.IsValid) 
            {

                var userService = _userService as UserService; 
                var response = await userService.Login(request); 
                if (response.IsSuccessful)
                    return RedirectToAction("Index", "Home");
                ModelState.AddModelError("", response.Message);
            }
            return View(); 
        }

        [Route("~/[action]")]
        public async Task<IActionResult> Logout()
        {
            var userService = _userService as UserService;
            await userService.Logout(); 
            return RedirectToAction(nameof(Login)); 
        }

        [Route("~/[action]")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Route("~/[action]")]
        public IActionResult Register(UserRegisterRequest request)
        {
            if (ModelState.IsValid) 
            {
                var userService = _userService as UserService;
                var response = userService.Register(request); 
                if (response.IsSuccessful)
                    return RedirectToAction(nameof(Login)); 
                ModelState.AddModelError("", response.Message); 
            }
            return View(request); 
        }

    }
}
