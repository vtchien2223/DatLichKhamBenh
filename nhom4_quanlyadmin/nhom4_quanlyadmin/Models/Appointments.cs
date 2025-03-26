namespace nhom4_quanlyadmin.Models
{
    public class Appointments
    {
        public int Id { get; set; }
        public TimeSpan? AppointmentDateStart { get; set; } 
        public TimeSpan? AppointmentDateEnd { get; set; }

        // Các thuộc tính chuỗi tạm thời để hiển thị trong View
        public string AppointmentDateStartString
        {
            get { return AppointmentDateStart?.ToString(@"hh\:mm\:ss"); }
            set { AppointmentDateStartString = value; }
        }

        public string AppointmentDateEndString
        {
            get { return AppointmentDateEnd?.ToString(@"hh\:mm\:ss"); }
            set { AppointmentDateEndString = value; }
        }
    }
}
