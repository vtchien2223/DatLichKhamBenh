using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom4_QLBA_API.Models;
using Nhom4_QLBA_API.Repositories;
using System;

namespace Nhom4_QLBA_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ApplicationDbContext _context; // Thêm DbContext để truy vấn trực tiếp

        public NotificationController(INotificationRepository notificationRepository, ApplicationDbContext context)
        {
            _notificationRepository = notificationRepository;
            _context = context; // Gán context
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotifications()
        {
            var notifications = await _notificationRepository.GetAllNotifications();
            return Ok(notifications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var notification = await _notificationRepository.GetNotificationById(id);
            if (notification == null)
                return NotFound();
            return Ok(notification);
        }

        [HttpPost]
        public async Task<IActionResult> AddNotification([FromBody] Notifications notification)
        {
            await _notificationRepository.AddNotification(notification);
            return CreatedAtAction(nameof(GetNotificationById), new { id = notification.Id }, notification);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(int id, [FromBody] Notifications notification)
        {
            if (id != notification.Id)
                return BadRequest();

            await _notificationRepository.UpdateNotification(notification);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            await _notificationRepository.DeleteNotification(id);
            return NoContent();
        }

        // Thêm API tạo thông báo tự động dựa trên ngày hẹn sắp đến (1 ngày)
        [HttpPost("AddUpcomingNotifications")]
        public async Task<IActionResult> AddUpcomingNotifications()
        {
            var today = DateTime.UtcNow.Date;

            // Lấy tất cả các lịch hẹn sắp đến (còn 1 ngày)
            var upcomingAppointments = await _context.AppointmentDetails
                .Where(a => a.AppointmentDate.HasValue &&
                            a.AppointmentDate.Value.Date == today.AddDays(1))
                .ToListAsync();

            if (!upcomingAppointments.Any())
                return NoContent(); // Không có lịch hẹn nào

            foreach (var appointment in upcomingAppointments)
            {
                // Tạo thông báo mới
                var notification = new Notifications
                {
                    AppointmentDetailId = appointment.Id,
                    Message = $"Bạn có cuộc hẹn với bác sĩ vào ngày {appointment.AppointmentDate.Value:dd/MM/yyyy}.",
                    SentAt = DateTime.UtcNow,
                    UrlImage = "https://example.com/default-image.png", // URL hình ảnh nếu cần
                    AppointmentDetail = appointment
                };

                _context.Notifications.Add(notification);
            }

            await _context.SaveChangesAsync(); // Lưu vào cơ sở dữ liệu

            return Ok("Notifications for upcoming appointments added successfully");
        }

        // Thêm API để lấy danh sách thông báo của người dùng
        [HttpGet("GetUserNotifications/{userName}")]
        public async Task<IActionResult> GetUserNotifications(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return BadRequest("UserName không hợp lệ.");

            var notifications = await _context.Notifications
                .Include(n => n.AppointmentDetail)
                .Where(n => n.AppointmentDetail.UserName == userName) // Lọc thông báo theo UserName
                .OrderByDescending(n => n.SentAt)
                .Select(n => new
                {
                    n.Message,
                    n.SentAt,
                    AppointmentDate = n.AppointmentDetail.AppointmentDate,
                })
                .ToListAsync();

            if (!notifications.Any())
                return NoContent(); // Không có thông báo nào

            return Ok(notifications); // Trả về danh sách thông báo
        }
    }
}
