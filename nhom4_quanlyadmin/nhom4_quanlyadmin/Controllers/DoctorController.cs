using Microsoft.AspNetCore.Mvc;
using nhom4_quanlyadmin.Models;
using nhom4_quanlyadmin.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace nhom4_quanlyadmin.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ApiService _apiService;

        // Inject ApiService thông qua constructor
        public DoctorController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Phương thức Index để hiển thị danh sách Doctor
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var doctors = await _apiService.GetAsync<List<Doctors>>("Doctor");
            var specialties = await _apiService.GetAsync<List<Specialty>>("Specialty");

            foreach (var doctor in doctors)
            {
                doctor.Specialty = specialties.FirstOrDefault(s => s.Id == doctor.SpecialtyId);
            }

            return View(doctors);
        }

        // Phương thức Create (GET) để hiển thị form Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var specialties = await _apiService.GetAsync<List<Specialty>>("Specialty");
            ViewBag.Specialties = specialties;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Doctors doctor)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.PostAsync("Doctor", doctor);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Doctor added successfully!";
                    // Chuyển về trang Index khi thành công
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to add doctor.YOU DO NOT HAVE PERMISSION TO CREATE";
                    return RedirectToAction(nameof(Index));
                }
            }

            var specialties = await _apiService.GetAsync<List<Specialty>>("Specialty");
            ViewBag.Specialties = specialties;
            return View(doctor);
        }


        // Phương thức Edit (GET) để hiển thị form Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _apiService.GetAsync<Doctors>($"Doctor/{id}");
            if (doctor == null)
            {
                return NotFound();
            }

            var specialties = await _apiService.GetAsync<List<Specialty>>("Specialty");
            ViewBag.Specialties = specialties;
            return View(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Doctors doctor)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.PutAsync($"Doctor/{doctor.Id}", doctor);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Doctor updated successfully!";
                    // Chuyển về trang Index khi thành công
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update doctor.YOU DO NOT HAVE PERMISSION TO UPDATE";
                    return RedirectToAction(nameof(Index));
                }
            }

            var specialties = await _apiService.GetAsync<List<Specialty>>("Specialty");
            ViewBag.Specialties = specialties;
            return View(doctor);
        }


        // Phương thức Delete (POST) để xử lý dữ liệu Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiService.DeleteAsync($"Doctor/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Doctor deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete doctor.YOU DO NOT HAVE PERMISSION TO DELETED";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
