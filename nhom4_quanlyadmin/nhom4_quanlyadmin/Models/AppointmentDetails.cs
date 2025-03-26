/*namespace nhom4_quanlyadmin.Models
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

        // Thay vì TimeSpan, giờ sẽ được lưu dưới dạng chuỗi
        public string AppointmentDateStart { get; set; } = string.Empty; // Chuỗi giờ bắt đầu
        public string AppointmentDateEnd { get; set; } = string.Empty;   // Chuỗi giờ kết thúc

        // Getter và Setter cho AppointmentDateStart, chuyển từ TimeSpan thành chuỗi
        public string AppointmentDateStartString
        {
            get
            {
                // Trả về giá trị AppointmentDateStart nếu có
                return string.IsNullOrEmpty(AppointmentDateStart) ? "" : AppointmentDateStart;
            }
            set
            {
                // Kiểm tra giá trị nhập vào và nếu hợp lệ thì lưu vào AppointmentDateStart
                if (!string.IsNullOrEmpty(value))
                {
                    // Nếu chỉ có giờ và phút "hh:mm", thêm ":00" vào để hoàn thiện giây
                    string formattedValue = value.Contains(":") ? value : value + ":00";

                    // Kiểm tra xem chuỗi có đúng định dạng giờ không
                    if (DateTime.TryParseExact(formattedValue, "HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out var startTime))
                    {
                        // Lưu giá trị hợp lệ vào AppointmentDateStart dưới dạng chuỗi
                        AppointmentDateStart = startTime.ToString("HH:mm:ss");
                    }
                    else
                    {
                        AppointmentDateStart = string.Empty; // Nếu không hợp lệ, đặt giá trị là rỗng
                    }
                }
                else
                {
                    AppointmentDateStart = string.Empty;
                }
            }
        }

        // Getter và Setter cho AppointmentDateEnd, chuyển từ TimeSpan thành chuỗi
        public string AppointmentDateEndString
        {
            get
            {
                // Trả về giá trị AppointmentDateEnd nếu có
                return string.IsNullOrEmpty(AppointmentDateEnd) ? "" : AppointmentDateEnd;
            }
            set
            {
                // Kiểm tra giá trị nhập vào và nếu hợp lệ thì lưu vào AppointmentDateEnd
                if (!string.IsNullOrEmpty(value))
                {
                    // Nếu chỉ có giờ và phút "hh:mm", thêm ":00" vào để hoàn thiện giây
                    string formattedValue = value.Contains(":") ? value : value + ":00";

                    // Kiểm tra xem chuỗi có đúng định dạng giờ không
                    if (DateTime.TryParseExact(formattedValue, "HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out var endTime))
                    {
                        // Lưu giá trị hợp lệ vào AppointmentDateEnd dưới dạng chuỗi
                        AppointmentDateEnd = endTime.ToString("HH:mm:ss");
                    }
                    else
                    {
                        AppointmentDateEnd = string.Empty; // Nếu không hợp lệ, đặt giá trị là rỗng
                    }
                }
                else
                {
                    AppointmentDateEnd = string.Empty;
                }
            }
        }

        // Mối quan hệ với các bảng khác
        public Doctors? Doctor { get; set; }
        public ServiceModel? Service { get; set; }
        public Appointments? Appointment { get; set; }
    }
}
*/
namespace nhom4_quanlyadmin.Models
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

        // Thời gian bắt đầu và kết thúc
        public TimeSpan? AppointmentDateStart { get; set; }
        public TimeSpan? AppointmentDateEnd { get; set; }

        // Mối quan hệ với các bảng khác
        public Doctors? Doctor { get; set; }
        public ServiceModel? Service { get; set; }
        public Appointments? Appointment { get; set; }
    }
}
