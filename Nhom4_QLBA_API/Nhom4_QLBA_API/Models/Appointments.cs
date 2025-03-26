namespace Nhom4_QLBA_API.Models
{
    public class Appointments
    {
        public int Id { get; set; }
        public TimeSpan? AppointmentDateStart { get; set; } // Chỉ giờ bắt đầu
        public TimeSpan? AppointmentDateEnd { get; set; }   // Chỉ giờ kết thúc
    }
}
