using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom4_QLBA_API.Models;
using Nhom4_QLBA_API.Repositories;

namespace Nhom4_QLBA_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentDetailsController : ControllerBase
    {
        private readonly IAppointmentDetailsRepository _appointmentDetailsRepository;

        public AppointmentDetailsController(IAppointmentDetailsRepository appointmentDetailsRepository)
        {
            _appointmentDetailsRepository = appointmentDetailsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _appointmentDetailsRepository.GetAllAppointments();
            return Ok(appointments);
        }

        // Lấy chi tiết một cuộc hẹn theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment = await _appointmentDetailsRepository.GetAppointmentById(id);
            if (appointment == null)
                return NotFound(new { message = $"Appointment with ID {id} not found." });

            var result = new
            {
                appointment.Id,
                appointment.UserName,
                appointment.AppointmentId,
                appointment.AppointmentDate,
                appointment.CreatedAt,
                AppointmentDateStart = appointment.AppointmentDateStart?.ToString(@"hh\:mm"),
                AppointmentDateEnd = appointment.AppointmentDateEnd?.ToString(@"hh\:mm"),
                Doctor = appointment.Doctor != null ? new { appointment.Doctor.Id, appointment.Doctor.FullName } : null,
                Service = appointment.Service != null ? new { appointment.Service.Id, appointment.Service.ServiceName } : null
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAppointment([FromBody] AppointmentDetails appointment)
        {
            if (appointment == null)
                return BadRequest("Appointment data is null.");

            await _appointmentDetailsRepository.AddAppointment(appointment);
            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.Id }, appointment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] AppointmentDetails appointment)
        {
            if (id != appointment.Id)
                return BadRequest("Appointment ID mismatch.");

            var existingAppointment = await _appointmentDetailsRepository.GetAppointmentById(id);
            if (existingAppointment == null)
                return NotFound();

            await _appointmentDetailsRepository.UpdateAppointment(appointment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var existingAppointment = await _appointmentDetailsRepository.GetAppointmentById(id);
            if (existingAppointment == null)
                return NotFound();

            await _appointmentDetailsRepository.DeleteAppointment(id);
            return NoContent();
        }

        [HttpGet("UpcomingAppointments")]
        public async Task<IActionResult> GetUpcomingAppointments([FromQuery] string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return BadRequest("UserName không hợp lệ.");

            var currentDate = DateTime.Now;

            var appointments = await _appointmentDetailsRepository.GetUpcomingAppointments(userName, currentDate);

            if (!appointments.Any())
                return NoContent(); // Trả về NoContent nếu không có cuộc hẹn

            return Ok(appointments);
        }


        [HttpGet("GetAppointmentsByUser")]
        public async Task<IActionResult> GetAppointmentsByUser([FromQuery] string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return BadRequest("UserName không hợp lệ.");

            // Sử dụng repository để truy vấn dữ liệu
            var userAppointments = await _appointmentDetailsRepository.GetAppointmentsByUser(userName);

            if (!userAppointments.Any())
            {
                return NoContent(); // Trả về 204 nếu không có cuộc hẹn cho người dùng
            }

            return Ok(userAppointments); // Trả về danh sách cuộc hẹn của người dùng
        }

        [HttpGet("GetAppointmentTimes/{appointmentId}")]
        public async Task<IActionResult> GetAppointmentTimes(int appointmentId)
        {
            var appointment = await _appointmentDetailsRepository.GetAppointmentTimesById(appointmentId);
            if (appointment == null)
            {
                return NotFound(new { message = $"Appointment with ID {appointmentId} not found." });
            }

            return Ok(new
            {
                startTime = appointment.AppointmentDateStart?.ToString(@"hh\:mm"),
                endTime = appointment.AppointmentDateEnd?.ToString(@"hh\:mm")
            });
        }


    }
}
