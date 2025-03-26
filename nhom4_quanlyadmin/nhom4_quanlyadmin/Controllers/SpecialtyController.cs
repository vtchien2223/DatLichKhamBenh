using Microsoft.AspNetCore.Mvc;
using nhom4_quanlyadmin.Models;
using nhom4_quanlyadmin.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nhom4_quanlyadmin.Controllers
{
    public class SpecialtyController : Controller
    {
        private readonly ApiService _apiService;

        // Inject ApiService vào controller
        public SpecialtyController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Hiển thị danh sách các chuyên khoa
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var specialties = await _apiService.GetAsync<List<Specialty>>("Specialty");
            return View(specialties);
        }

        // Hiển thị form tạo chuyên khoa mới
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý tạo chuyên khoa mới
        [HttpPost]
        public async Task<IActionResult> Create(Specialty specialty)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.PostAsync("Specialty", specialty);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Specialty added successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to add specialty.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(specialty);
        }

        // Hiển thị form sửa chuyên khoa
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var specialty = await _apiService.GetAsync<Specialty>($"Specialty/{id}");
            if (specialty == null)
            {
                return NotFound();
            }
            return View(specialty);
        }

        // Xử lý sửa chuyên khoa
        [HttpPost]
        public async Task<IActionResult> Edit(Specialty specialty)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.PutAsync($"Specialty/{specialty.Id}", specialty);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Specialty updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update specialty.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(specialty);
        }

        // Xóa chuyên khoa
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiService.DeleteAsync($"Specialty/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Specialty deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete specialty.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
