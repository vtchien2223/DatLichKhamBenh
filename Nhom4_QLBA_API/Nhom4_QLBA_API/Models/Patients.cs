namespace Nhom4_QLBA_API.Models
{
    public class Patients
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? UrlAvatar { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreatedAt { get; set; }

    }

}
