using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nhom4_QLBA_API.Models;
using Nhom4_QLBA_API.Repositories;

namespace Nhom4_QLBA_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorController(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctorRepository.GetAllDoctors();
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _doctorRepository.GetDoctorById(id);
            if (doctor == null)
                return NotFound();

            return Ok(doctor);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddDoctor([FromBody] Doctors doctor)
        {
            await _doctorRepository.AddDoctor(doctor);
            return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.Id }, doctor);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] Doctors doctor)
        {
            if (id != doctor.Id)
                return BadRequest();

            await _doctorRepository.UpdateDoctor(doctor);
            return NoContent();
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            await _doctorRepository.DeleteDoctor(id);
            return NoContent();
        }
    }


}
