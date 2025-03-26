using Microsoft.AspNetCore.Mvc;
using nhom4_quanlyadmin.Models;
using nhom4_quanlyadmin.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nhom4_quanlyadmin.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApiService _apiService;

        // Inject ApiService vào controller
        public ServiceController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Hiển thị danh sách các dịch vụ
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var services = await _apiService.GetAsync<List<ServiceModel>>("Service");
            return View(services);
        }

        // Hiển thị form tạo dịch vụ mới
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceModel service)
        {
            if (ModelState.IsValid)
            {
                // Sử dụng giá trị mặc định 0 nếu Price là null
                service.Price = Math.Round(service.Price ?? 0, 2);  // Nếu Price là null, sẽ gán giá trị mặc định là 0

                var response = await _apiService.PostAsync("Service", service);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Service added successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to add service.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(service);
        }

        // Hiển thị form sửa dịch vụ
        // Hiển thị form sửa dịch vụ
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var service = await _apiService.GetAsync<ServiceModel>($"Service/{id}");
            if (service == null)
            {
                // Nếu không tìm thấy dịch vụ, trả về lỗi
                return NotFound();
            }

            // Đảm bảo rằng các thuộc tính trong service không null
            if (service.Price == null)
            {
                service.Price = 0; // Nếu price null, gán giá trị mặc định
            }

            return View(service);
        }

        // Xử lý sửa dịch vụ
        [HttpPost]
        public async Task<IActionResult> Edit(ServiceModel service)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.PutAsync($"Service/{service.Id}", service);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Service updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update service.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(service);
        }

        // Xóa dịch vụ
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiService.DeleteAsync($"Service/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Service deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete service.";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
