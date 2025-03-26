using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notifications>> GetAllNotifications();
        Task<Notifications> GetNotificationById(int id);
        Task AddNotification(Notifications notification);
        Task UpdateNotification(Notifications notification);
        Task DeleteNotification(int id);
    }

}
