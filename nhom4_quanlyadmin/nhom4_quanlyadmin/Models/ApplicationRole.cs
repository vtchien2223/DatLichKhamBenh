using Microsoft.AspNetCore.Identity;

namespace Nhom4_QLBA_API.Models
{
	public class ApplicationRole : IdentityRole
	{
		// Thêm các cột đại diện cho từng chức năng
		public bool CanManageDoctors { get; set; }
		public bool CanManagePatients { get; set; }
		public bool CanManageAppointments { get; set; }
		public bool CanManageServices { get; set; }
		public bool CanManageSpecialty { get; set; }
		public bool CanManageAppointmentDetails { get; set; }
		public bool CanManagePosts { get; set; }
	}
}