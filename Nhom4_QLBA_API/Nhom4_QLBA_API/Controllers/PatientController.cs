using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nhom4_QLBA_API.Models;
using Nhom4_QLBA_API.Repositories;

namespace Nhom4_QLBA_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        [Authorize(Policy = "CanManagePatients")]
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientRepository.GetAllPatients();
            return Ok(patients);
        }
        [Authorize(Policy = "CanManagePatients")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await _patientRepository.GetPatientById(id);
            if (patient == null)
                return NotFound();

            return Ok(patient);
        }
        [Authorize(Policy = "CanManagePatients")]
        [HttpPost]
        public async Task<IActionResult> AddPatient([FromBody] Patients patient)
        {
            await _patientRepository.AddPatient(patient);
            return CreatedAtAction(nameof(GetPatientById), new { id = patient.Id }, patient);
        }
        [Authorize(Policy = "CanManagePatients")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patients patient)
        {
            if (id != patient.Id)
                return BadRequest();

            await _patientRepository.UpdatePatient(patient);
            return NoContent();
        }
        [Authorize(Policy = "CanManagePatients")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            await _patientRepository.DeletePatient(id);
            return NoContent();
        }
    }

}
