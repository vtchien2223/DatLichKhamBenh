using Microsoft.EntityFrameworkCore;
using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notifications>> GetAllNotifications()
        {
            return await _context.Notifications
                .Include(n => n.AppointmentDetail)
                .ToListAsync();
        }

        public async Task<Notifications> GetNotificationById(int id)
        {
            return await _context.Notifications
                .Include(n => n.AppointmentDetail)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task AddNotification(Notifications notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNotification(Notifications notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNotification(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }
    }

}
