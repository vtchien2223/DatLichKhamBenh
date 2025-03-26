using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Repositories
{
    public interface IMedicalRecordRepository
    {
        Task<IEnumerable<MedicalRecordDetails>> GetAllMedicalRecords();
        Task<MedicalRecordDetails> GetMedicalRecordById(int id);
        Task AddMedicalRecord(MedicalRecordDetails medicalRecord);
        Task UpdateMedicalRecord(MedicalRecordDetails medicalRecord);
        Task DeleteMedicalRecord(int id);
    }
}
