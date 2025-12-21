#nullable disable
using APP.Models;
using CORE.APP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IService<RoleRequest, RoleResponse> _roleService;

        public RoleController(IService<RoleRequest, RoleResponse> roleService)
        {
            _roleService = roleService;
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        public IActionResult Index()
        {
            var list = _roleService.List();
            return View(list);
        }

        public IActionResult Details(int id)
        {
            var item = _roleService.Item(id);
            if (item == null)
            {
                SetTempData("Role not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(RoleRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = _roleService.Create(request);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Index), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            return View(request);
        }

        public IActionResult Edit(int id)
        {
            var request = _roleService.Edit(id);
            if (request == null)
            {
                SetTempData("Role not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(RoleRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = _roleService.Update(request);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            return View(request);
        }

        public IActionResult Delete(int id)
        {
            var item = _roleService.Item(id);
            if (item == null)
            {
                SetTempData("Role not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(RoleResponse role)
        {
            if (role is null)
                return RedirectToAction(nameof(Index));

            var response = _roleService.Delete(role.Id);
            TempData["Message"] = response.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
