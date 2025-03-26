using Microsoft.EntityFrameworkCore;
using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Repositories
{
    public class AppointmentDetailsRepository : IAppointmentDetailsRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentDetailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /*public async Task<IEnumerable<AppointmentDetails>> GetAllAppointments()
        {
            // Bao gồm thông tin Appointment để truy xuất được các thông tin như AppointmentDateStart và AppointmentDateEnd
            return await _context.AppointmentDetails
                .Include(a => a.Doctor)
                .Include(a => a.Service)
                .Include(a => a.Appointment)  // Bao gồm Appointment để lấy dữ liệu giờ bắt đầu và kết thúc
                .ToListAsync();
        }

        public async Task<AppointmentDetails> GetAppointmentById(int id)
        {
            return await _context.AppointmentDetails
                .Include(a => a.Doctor)
                .Include(a => a.Service)
                .Include(a => a.Appointment)
                .FirstOrDefaultAsync(a => a.Id == id);
        }*/

        public async Task<IEnumerable<AppointmentDetails>> GetAllAppointments()
        {
            return await _context.AppointmentDetails
                .Include(a => a.Doctor)
                .Include(a => a.Service)
                .Include(a => a.Appointment)
                .Select(a => new AppointmentDetails
                {
                    Id = a.Id,
                    UserName = a.UserName,
                    DoctorId = a.DoctorId,
                    ServiceId = a.ServiceId,
                    AppointmentId = a.AppointmentId,
                    AppointmentDate = a.AppointmentDate,
                    CreatedAt = a.CreatedAt,
                    AppointmentDateStart = a.Appointment != null ? a.Appointment.AppointmentDateStart : null,
                    AppointmentDateEnd = a.Appointment != null ? a.Appointment.AppointmentDateEnd : null,
                    Doctor = a.Doctor,
                    Service = a.Service
                })
                .ToListAsync();
        }

        public async Task<AppointmentDetails> GetAppointmentById(int id)
        {
            var appointmentDetails = await _context.AppointmentDetails
                .Include(a => a.Doctor)
                .Include(a => a.Service)
                .Include(a => a.Appointment)
                .Select(a => new AppointmentDetails
                {
                    Id = a.Id,
                    UserName = a.UserName,
                    DoctorId = a.DoctorId,
                    ServiceId = a.ServiceId,
                    AppointmentId = a.AppointmentId,
                    AppointmentDate = a.AppointmentDate,
                    CreatedAt = a.CreatedAt,
                    AppointmentDateStart = a.Appointment != null ? a.Appointment.AppointmentDateStart : null,
                    AppointmentDateEnd = a.Appointment != null ? a.Appointment.AppointmentDateEnd : null,
                    Doctor = a.Doctor,
                    Service = a.Service
                })
                .FirstOrDefaultAsync(a => a.Id == id);

            return appointmentDetails;
        }



        public async Task AddAppointment(AppointmentDetails appointment)
        {
            await _context.AppointmentDetails.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointment(AppointmentDetails appointment)
        {
            _context.AppointmentDetails.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointment(int id)
        {
            var appointment = await _context.AppointmentDetails.FindAsync(id);
            if (appointment != null)
            {
                _context.AppointmentDetails.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }

        /* public async Task<IEnumerable<AppointmentDetails>> GetUpcomingAppointments(string userName, DateTime currentDate)
         {
             return await _context.AppointmentDetails
                 .Include(a => a.Appointment)  // Include related Appointment details
                 .Include(a => a.Service)      // Include related Service details
                 .Include(a => a.Doctor)       // Include related Doctor details
                 .Where(a => a.UserName == userName // Filter by UserName in AppointmentDetails
                             && a.AppointmentDate.HasValue // Ensure AppointmentDate is not null
                             && a.AppointmentDate.Value.Date == currentDate.Date.AddDays(1)) // Filter for the next day
                 .ToListAsync();
         }

         public async Task<IEnumerable<AppointmentDetails>> GetAppointmentsByUser(string userName)
         {
             return await _context.AppointmentDetails
                 .Include(a => a.Appointment)
                 .Include(a => a.Service)
                 .Include(a => a.Doctor)
                 .Where(a => a.UserName == userName)
                 .ToListAsync();
         }*/

        public async Task<IEnumerable<AppointmentDetails>> GetUpcomingAppointments(string userName, DateTime currentDate)
        {
            return await _context.AppointmentDetails
                .Include(a => a.Appointment)  // Include Appointment để lấy StartTime và EndTime
                .Include(a => a.Service)      // Include Service để lấy thông tin dịch vụ
                .Include(a => a.Doctor)       // Include Doctor để lấy thông tin bác sĩ
                .Where(a => a.UserName == userName
                            && a.AppointmentDate.HasValue
                            && a.AppointmentDate.Value.Date == currentDate.Date.AddDays(1)) // Lọc các cuộc hẹn vào ngày tiếp theo
                .Select(a => new AppointmentDetails
                {
                    Id = a.Id,
                    UserName = a.UserName,
                    DoctorId = a.DoctorId,
                    ServiceId = a.ServiceId,
                    AppointmentId = a.AppointmentId,
                    AppointmentDate = a.AppointmentDate,
                    CreatedAt = a.CreatedAt,
                    AppointmentDateStart = a.Appointment != null ? a.Appointment.AppointmentDateStart : null,
                    AppointmentDateEnd = a.Appointment != null ? a.Appointment.AppointmentDateEnd : null,
                    Doctor = a.Doctor,
                    Service = a.Service
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentDetails>> GetAppointmentsByUser(string userName)
        {
            return await _context.AppointmentDetails
                .Include(a => a.Appointment)  // Include Appointment để lấy StartTime và EndTime
                .Include(a => a.Service)      // Include Service để lấy thông tin dịch vụ
                .Include(a => a.Doctor)       // Include Doctor để lấy thông tin bác sĩ
                .Where(a => a.UserName == userName)  // Lọc theo UserName
                .Select(a => new AppointmentDetails
                {
                    Id = a.Id,
                    UserName = a.UserName,
                    DoctorId = a.DoctorId,
                    ServiceId = a.ServiceId,
                    AppointmentId = a.AppointmentId,
                    AppointmentDate = a.AppointmentDate,
                    CreatedAt = a.CreatedAt,
                    AppointmentDateStart = a.Appointment != null ? a.Appointment.AppointmentDateStart : null,
                    AppointmentDateEnd = a.Appointment != null ? a.Appointment.AppointmentDateEnd : null,
                    Doctor = a.Doctor,
                    Service = a.Service
                })
                .ToListAsync();
        }

        public async Task<Appointments> GetAppointmentTimesById(int appointmentId)
        {
            return await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);
        }


    }
}
