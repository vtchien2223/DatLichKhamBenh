namespace nhom4_quanlyadmin.Models
{
    public class Doctors
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int SpecialtyId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UrlAvatar { get; set; }

        // Thêm thuộc tính để liên kết Specialty
        public Specialty? Specialty { get; set; }
    }
}
