using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nhom4_QLBA_API.Models;
using Nhom4_QLBA_API.Repositories;

namespace Nhom4_QLBA_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }
        [Authorize(Policy = "CanManageServices")]
        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _serviceRepository.GetAllServices();
            return Ok(services);
        }
        [Authorize(Policy = "CanManageServices")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var service = await _serviceRepository.GetServiceById(id);
            if (service == null)
                return NotFound();

            return Ok(service);
        }
        [Authorize(Policy = "CanManageServices")]
        [HttpPost]
        public async Task<IActionResult> AddService([FromBody] Services service)
        {
            await _serviceRepository.AddService(service);
            return CreatedAtAction(nameof(GetServiceById), new { id = service.Id }, service);
        }
        [Authorize(Policy = "CanManageServices")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, [FromBody] Services service)
        {
            if (id != service.Id)
                return BadRequest();

            await _serviceRepository.UpdateService(service);
            return NoContent();
        }
        [Authorize(Policy = "CanManageServices")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            await _serviceRepository.DeleteService(id);
            return NoContent();
        }
    }

}
