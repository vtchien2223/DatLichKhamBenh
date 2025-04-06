using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize(Roles = "Admin")] // Chỉ admin mới có thể quản lý quyền
	public class RoleController : ControllerBase
	{
		private readonly RoleManager<ApplicationRole> _roleManager;

		public RoleController(RoleManager<ApplicationRole> roleManager)
		{
			_roleManager = roleManager;
		}

		// Lấy danh sách tất cả role và quyền của chúng
		[HttpGet]
		public async Task<IActionResult> GetAllRoles()
		{
			var roles = _roleManager.Roles.ToList();
			return Ok(roles);
		}

		// Lấy thông tin quyền của một role cụ thể
		[HttpGet("{roleId}")]
		public async Task<IActionResult> GetRolePermissions(string roleId)
		{
			var role = await _roleManager.FindByIdAsync(roleId);
			if (role == null)
				return NotFound();

			return Ok(new RolePermissionDto
			{
				RoleId = role.Id,
				CanManageDoctors = role.CanManageDoctors,
				CanManagePatients = role.CanManagePatients,
				CanManageAppointments = role.CanManageAppointments,
				CanManageServices = role.CanManageServices,
				CanManageSpecialty = role.CanManageSpecialty,
				CanManageAppointmentDetails = role.CanManageAppointmentDetails,
				CanManagePosts = role.CanManagePosts
			});
		}

		// Cập nhật quyền cho một role
		[HttpPut("{roleId}")]
		public async Task<IActionResult> UpdateRolePermissions(string roleId, [FromBody] RolePermissionDto permissions)
		{
			if (roleId != permissions.RoleId)
				return BadRequest("Role ID mismatch");

			var role = await _roleManager.FindByIdAsync(roleId);
			if (role == null)
				return NotFound();

			// Cập nhật quyền
			role.CanManageDoctors = permissions.CanManageDoctors;
			role.CanManagePatients = permissions.CanManagePatients;
			role.CanManageAppointments = permissions.CanManageAppointments;
			role.CanManageServices = permissions.CanManageServices;
			role.CanManageSpecialty = permissions.CanManageSpecialty;
			role.CanManageAppointmentDetails = permissions.CanManageAppointmentDetails;
			role.CanManagePosts = permissions.CanManagePosts;

			var result = await _roleManager.UpdateAsync(role);
			if (!result.Succeeded)
				return BadRequest(result.Errors);

			return NoContent();
		}
	}
}