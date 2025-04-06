using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nhom4_QLBA_API.Models;
using Nhom4_QLBA_API.Repositories;

namespace Nhom4_QLBA_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyRepository _specialtyRepository;

        public SpecialtyController(ISpecialtyRepository specialtyRepository)
        {
            _specialtyRepository = specialtyRepository;
        }
        [Authorize(Policy = "CanManageSpecialty")]
        [HttpGet]
        public async Task<IActionResult> GetAllSpecialties()
        {
            var specialties = await _specialtyRepository.GetAllSpecialties();
            return Ok(specialties);
        }
        [Authorize(Policy = "CanManageSpecialty")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecialtyById(int id)
        {
            var specialty = await _specialtyRepository.GetSpecialtyById(id);
            if (specialty == null)
            {
                return NotFound();
            }
            return Ok(specialty);
        }
        [Authorize(Policy = "CanManageSpecialty")]
        [HttpPost]
        public async Task<IActionResult> AddSpecialty([FromBody] Specialty specialty)
        {
            if (specialty == null)
            {
                return BadRequest();
            }

            await _specialtyRepository.AddSpecialty(specialty);
            return CreatedAtAction(nameof(GetSpecialtyById), new { id = specialty.Id }, specialty);
        }
        [Authorize(Policy = "CanManageSpecialty")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpecialty(int id, [FromBody] Specialty specialty)
        {
            if (id != specialty.Id)
            {
                return BadRequest();
            }

            await _specialtyRepository.UpdateSpecialty(specialty);
            return NoContent();
        }
        [Authorize(Policy = "CanManageSpecialty")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecialty(int id)
        {
            await _specialtyRepository.DeleteSpecialty(id);
            return NoContent();
        }
    }
}
