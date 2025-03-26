using Microsoft.AspNetCore.Mvc;
using nhom4_quanlyadmin.Models;
using nhom4_quanlyadmin.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace nhom4_quanlyadmin.Controllers
{
    public class AppointmentDetailsController : Controller
    {
        private readonly ApiService _apiService;

        public AppointmentDetailsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // ✅ Hiển thị danh sách các cuộc hẹn
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var appointmentDetails = await _apiService.GetAsync<List<AppointmentDetails>>("AppointmentDetails");
            return View(appointmentDetails);
        }

        // ✅ Hiển thị form thêm cuộc hẹn mới
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdownData();
            return View(new AppointmentDetails());
        }

        // ✅ Xử lý thêm cuộc hẹn mới
        [HttpPost]
        public async Task<IActionResult> Create(AppointmentDetails appointmentDetail)
        {
            if (ModelState.IsValid)
            {
                // Gán ngày tạo là thời gian hiện tại nếu chưa có
                if (appointmentDetail.CreatedAt == DateTime.MinValue)
                {
                    appointmentDetail.CreatedAt = DateTime.Now;
                }

                // Gán ngày hẹn mặc định là hôm nay nếu chưa chọn
                if (appointmentDetail.AppointmentDate == null)
                {
                    appointmentDetail.AppointmentDate = DateTime.Today;
                }

                // Gửi yêu cầu POST đến API
                var response = await _apiService.PostAsync("AppointmentDetails", appointmentDetail);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Thêm chi tiết cuộc hẹn thành công!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Thêm chi tiết cuộc hẹn thất bại!";
                }
            }

            await LoadDropdownData();
            return View(appointmentDetail);
        }


        // Xử lý GET cho Edit
        public async Task<IActionResult> Edit(int id)
        {
            // Fetch the appointment details by ID from the API
            var appointmentDetail = await _apiService.GetAsync<AppointmentDetails>($"AppointmentDetails/{id}");

            if (appointmentDetail == null)
            {
                return NotFound();
            }

            // Load dropdown data
            var doctors = await _apiService.GetAsync<List<Doctors>>("Doctor");
            var services = await _apiService.GetAsync<List<ServiceModel>>("Service");
            var appointments = await _apiService.GetAsync<List<Appointments>>("Appointment");

            // Check if the necessary data is available
            if (doctors == null || services == null || appointments == null)
            {
                TempData["ErrorMessage"] = "Cannot load required data.";
                return RedirectToAction(nameof(Index));
            }

            // Pass the data to the View
            ViewBag.Doctors = doctors;
            ViewBag.Services = services;
            ViewBag.Appointments = appointments;

            // Return the view with the model populated
            return View(appointmentDetail);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(AppointmentDetails appointmentDetail)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.PutAsync($"AppointmentDetails/{appointmentDetail.Id}", appointmentDetail);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật chi tiết cuộc hẹn thành công!";
                    return RedirectToAction(nameof(Index));
                }
                TempData["ErrorMessage"] = "Cập nhật chi tiết cuộc hẹn thất bại.";
                return RedirectToAction(nameof(Index));
            }

            // Nếu dữ liệu không hợp lệ, tải lại các dữ liệu cho dropdown
            await LoadDropdownData();
            return View(appointmentDetail); // Trả về lại View nếu dữ liệu không hợp lệ
        }


        // API để lấy giờ bắt đầu và giờ kết thúc theo AppointmentId
        [HttpGet]
        public async Task<JsonResult> GetAppointmentTimes(int appointmentId)
        {
            var appointment = await _apiService.GetAsync<Appointments>($"Appointment/{appointmentId}");
            if (appointment != null)
            {
                return Json(new
                {
                    startTime = appointment.AppointmentDateStart?.ToString(@"hh\:mm"),
                    endTime = appointment.AppointmentDateEnd?.ToString(@"hh\:mm")
                });
            }

            return Json(null);
        }

        // Load dữ liệu cho các dropdown (bác sĩ, dịch vụ, cuộc hẹn)
        private async Task LoadDropdownData()
        {
            var doctors = await _apiService.GetAsync<List<Doctors>>("Doctor");
            var services = await _apiService.GetAsync<List<ServiceModel>>("Service");
            var appointments = await _apiService.GetAsync<List<Appointments>>("Appointment");

            ViewBag.Doctors = doctors;
            ViewBag.Services = services;
            ViewBag.Appointments = appointments;
        }



        // ✅ Xử lý xóa cuộc hẹn
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiService.DeleteAsync($"AppointmentDetails/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Xóa chi tiết cuộc hẹn thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Xóa chi tiết cuộc hẹn thất bại!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
