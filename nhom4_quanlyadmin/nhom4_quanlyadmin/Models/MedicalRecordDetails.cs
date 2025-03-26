namespace nhom4_quanlyadmin.Models
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
