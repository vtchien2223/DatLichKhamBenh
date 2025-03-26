using Nhom4_QLBA_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nhom4_QLBA_API.Repositories
{
    public interface IAppointmentDetailsRepository
    {
        Task<IEnumerable<AppointmentDetails>> GetAllAppointments();
        Task<AppointmentDetails> GetAppointmentById(int id);
        Task AddAppointment(AppointmentDetails appointment);
        Task UpdateAppointment(AppointmentDetails appointment);
        Task DeleteAppointment(int id);
        Task<IEnumerable<AppointmentDetails>> GetUpcomingAppointments(string userName, DateTime currentDate);
        Task<IEnumerable<AppointmentDetails>> GetAppointmentsByUser(string userName);
        Task<Appointments> GetAppointmentTimesById(int appointmentId);


    }
}
