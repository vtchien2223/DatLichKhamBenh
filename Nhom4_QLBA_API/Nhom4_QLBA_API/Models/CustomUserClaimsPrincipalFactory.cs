using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Nhom4_QLBA_API.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Nhom4_QLBA_API
{
	public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, ApplicationRole>
	{
		private readonly RoleManager<ApplicationRole> _roleManager;

		public CustomUserClaimsPrincipalFactory(
			UserManager<User> userManager,
			RoleManager<ApplicationRole> roleManager,
			IOptions<IdentityOptions> optionsAccessor)
			: base(userManager, roleManager, optionsAccessor)
		{
			_roleManager = roleManager;
		}

		protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
		{
			var identity = await base.GenerateClaimsAsync(user);
			var roles = await UserManager.GetRolesAsync(user);

			foreach (var roleName in roles)
			{
				var role = await _roleManager.FindByNameAsync(roleName);
				if (role != null)
				{
					// Thêm các quyền vào claims
					if (role.CanManageDoctors)
						identity.AddClaim(new Claim("Permission", "CanManageDoctors"));
					if (role.CanManagePatients)
						identity.AddClaim(new Claim("Permission", "CanManagePatients"));
					if (role.CanManageAppointments)
						identity.AddClaim(new Claim("Permission", "CanManageAppointments"));
					if (role.CanManageServices)
						identity.AddClaim(new Claim("Permission", "CanManageServices"));
					if (role.CanManageSpecialty)
						identity.AddClaim(new Claim("Permission", "CanManageSpecialty"));
					if (role.CanManageAppointmentDetails)
						identity.AddClaim(new Claim("Permission", "CanManageAppointmentDetails"));
					if (role.CanManagePosts)
						identity.AddClaim(new Claim("Permission", "CanManagePosts"));
				}
			}
			return identity;
		}
	}
}