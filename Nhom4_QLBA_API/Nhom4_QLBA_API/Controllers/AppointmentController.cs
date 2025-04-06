using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nhom4_QLBA_API.Models;
using Nhom4_QLBA_API.Repositories;

namespace Nhom4_QLBA_API
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentController(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        [Authorize(Policy = "CanManageAppointments")]
        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _appointmentRepository.GetAllAppointments();
            return Ok(appointments);
        }
        [Authorize(Policy = "CanManageAppointments")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment = await _appointmentRepository.GetAppointmentById(id);
            if (appointment == null)
                return NotFound();

            // Đảm bảo các thành viên kiểu ẩn danh được gán đúng cách
            var result = new
            {
                appointment.Id,
                AppointmentDateStart = appointment.AppointmentDateStart?.ToString(@"hh\:mm\:ss"),  // Gán đúng cách tên và giá trị
                AppointmentDateEnd = appointment.AppointmentDateEnd?.ToString(@"hh\:mm\:ss")    // Gán đúng cách tên và giá trị
            };

            return Ok(result);
        }
        [Authorize(Policy = "CanManageAppointments")]
        [HttpPost]
        public async Task<IActionResult> AddAppointment([FromBody] Appointments appointment)
        {
            await _appointmentRepository.AddAppointment(appointment);
            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.Id }, appointment);
        }
        [Authorize(Policy = "CanManageAppointments")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] Appointments appointment)
        {
            if (id != appointment.Id)
                return BadRequest();

            await _appointmentRepository.UpdateAppointment(appointment);
            return NoContent();
        }
        [Authorize(Policy = "CanManageAppointments")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            await _appointmentRepository.DeleteAppointment(id);
            return NoContent();
        }
    }

}


