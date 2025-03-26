using Microsoft.EntityFrameworkCore;
using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Jobs
{
    public class NotificationJob
    {
        private readonly ApplicationDbContext _context;

        public NotificationJob(ApplicationDbContext context)
        {
            _context = context; // Inject DbContext để truy vấn dữ liệu
        }

        // Hàm xử lý logic thông báo
        public async Task AddUpcomingNotifications()
        {
            var today = DateTime.UtcNow.Date;

            // Tìm các cuộc hẹn cách 1 ngày
            var upcomingAppointments = await _context.AppointmentDetails
                .Where(a => a.AppointmentDate.HasValue &&
                            a.AppointmentDate.Value.Date == today.AddDays(1))
                .ToListAsync();

            if (!upcomingAppointments.Any())
            {
                Console.WriteLine("Không có lịch hẹn nào trong ngày mai."); // Log
                return;
            }

            foreach (var appointment in upcomingAppointments)
            {
                // Tạo một thông báo mới
                var notification = new Notifications
                {
                    AppointmentDetailId = appointment.Id,
                    Message = $"Bạn có cuộc hẹn vào ngày {appointment.AppointmentDate.Value:dd/MM/yyyy}.",
                    SentAt = DateTime.UtcNow,
                    UrlImage = "https://example.com/default-image.png", // URL ảnh mặc định
                    AppointmentDetail = appointment
                };

                _context.Notifications.Add(notification);
            }

            // Lưu thông báo vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            Console.WriteLine("Thông báo đã được tạo tự động cho các lịch hẹn vào ngày mai."); // Log
        }
    }
}
