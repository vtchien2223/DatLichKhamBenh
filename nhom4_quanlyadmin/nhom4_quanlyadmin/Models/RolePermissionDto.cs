namespace Nhom4_QLBA_API.Models
{
	public class RolePermissionDto
	{
		public string RoleId { get; set; }
		public bool CanManageDoctors { get; set; }
		public bool CanManagePatients { get; set; }
		public bool CanManageAppointments { get; set; }
		public bool CanManageServices { get; set; }
		public bool CanManageSpecialty { get; set; }
		public bool CanManageAppointmentDetails { get; set; }
		public bool CanManagePosts { get; set; }
	}
}