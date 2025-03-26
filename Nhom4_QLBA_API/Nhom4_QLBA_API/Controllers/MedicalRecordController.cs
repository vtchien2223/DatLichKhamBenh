using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nhom4_QLBA_API.Models;
using Nhom4_QLBA_API.Repositories;

namespace Nhom4_QLBA_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public MedicalRecordController(IMedicalRecordRepository medicalRecordRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMedicalRecords()
        {
            var medicalRecords = await _medicalRecordRepository.GetAllMedicalRecords();
            return Ok(medicalRecords);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicalRecordById(int id)
        {
            var medicalRecord = await _medicalRecordRepository.GetMedicalRecordById(id);
            if (medicalRecord == null)
                return NotFound();

            return Ok(medicalRecord);
        }

        [HttpPost]
        public async Task<IActionResult> AddMedicalRecord([FromBody] MedicalRecordDetails medicalRecord)
        {
            await _medicalRecordRepository.AddMedicalRecord(medicalRecord);
            return CreatedAtAction(nameof(GetMedicalRecordById), new { id = medicalRecord.Id }, medicalRecord);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicalRecord(int id, [FromBody] MedicalRecordDetails medicalRecord)
        {
            if (id != medicalRecord.Id)
                return BadRequest();

            await _medicalRecordRepository.UpdateMedicalRecord(medicalRecord);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalRecord(int id)
        {
            await _medicalRecordRepository.DeleteMedicalRecord(id);
            return NoContent();
        }
    }

}
