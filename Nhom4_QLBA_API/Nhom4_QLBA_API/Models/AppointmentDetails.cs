/*namespace Nhom4_QLBA_API.Models
{
    public class AppointmentDetails
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        //public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int ServiceId { get; set; }
        public int AppointmentId { get; set; }

        public DateTime? AppointmentDate { get; set; }
        public DateTime CreatedAt { get; set; }

        // Mối quan hệ với các bảng khác
        //public Patients? Patient { get; set; }
        public Doctors? Doctor { get; set; }
        public Services? Service { get; set; }
        public Appointments? Appointment { get; set; }
       
    }
}
*/

namespace Nhom4_QLBA_API.Models
{
    public class AppointmentDetails
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public int DoctorId { get; set; }
        public int ServiceId { get; set; }
        public int AppointmentId { get; set; }

        public DateTime? AppointmentDate { get; set; }
        public DateTime CreatedAt { get; set; }

        // Thêm các thuộc tính mới để lưu thời gian bắt đầu và kết thúc
        public TimeSpan? AppointmentDateStart { get; set; }
        public TimeSpan? AppointmentDateEnd { get; set; }

        // Mối quan hệ với các bảng khác
        public Doctors? Doctor { get; set; }
        public Services? Service { get; set; }
        public Appointments? Appointment { get; set; }
    }
}
