using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Models
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointments>> GetAllAppointments();
        Task<Appointments> GetAppointmentById(int id);
        Task AddAppointment(Appointments appointment);
        Task UpdateAppointment(Appointments appointment);
        Task DeleteAppointment(int id);
    }

}
