using Microsoft.AspNetCore.Mvc;
using nhom4_quanlyadmin.Models;
using nhom4_quanlyadmin.Services;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace nhom4_quanlyadmin.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApiService _apiService;

        // Inject ApiService vào controller
        public PatientController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Hiển thị danh sách các bệnh nhân
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var patients = await _apiService.GetAsync<List<Patients>>("Patient");
            return View(patients);
        }

        // Hiển thị form tạo bệnh nhân mới
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý tạo bệnh nhân mới
        public async Task<IActionResult> Create(Patients patient)
        {
            if (ModelState.IsValid)
            {
                // Gán ngày giờ hiện tại cho CreatedAt
                patient.CreatedAt = DateTime.Now;  // Hoặc DateTime.UtcNow nếu cần theo giờ UTC

                var response = await _apiService.PostAsync("Patient", patient);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Patient added successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to add patient.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(patient);
        }



        // Hiển thị form sửa thông tin bệnh nhân
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _apiService.GetAsync<Patients>($"Patient/{id}");
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // Xử lý sửa thông tin bệnh nhân
        [HttpPost]
        public async Task<IActionResult> Edit(Patients patient)
        {
            if (ModelState.IsValid)
            {
                // Chỉ chuyển đổi ngày nếu nó không phải là null
                if (patient.CreatedAt.HasValue)
                {
                    // Nếu có giá trị, kiểm tra và chuyển đổi nó
                    patient.CreatedAt = DateTime.ParseExact(patient.CreatedAt.Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                var response = await _apiService.PutAsync($"Patient/{patient.Id}", patient);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Patient updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update patient.";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(patient);
        }


        // Xóa bệnh nhân
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiService.DeleteAsync($"Patient/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Patient deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete patient.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
