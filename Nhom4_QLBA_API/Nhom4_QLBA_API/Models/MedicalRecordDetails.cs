namespace Nhom4_QLBA_API.Models
{
    public class MedicalRecordDetails
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string? Treatment { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Patients? Patient { get; set; }
    }
}
