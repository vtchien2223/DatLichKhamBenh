namespace nhom4_quanlyadmin.Models
{
    public class Notifications
    {
        public int Id { get; set; }
        public int? AppointmentDetailId { get; set; }
        public string? Message { get; set; }
        public DateTime? SentAt { get; set; }

        public string? UrlImage { get; set; }
        public AppointmentDetails? AppointmentDetail { get; set; }
    }

}
