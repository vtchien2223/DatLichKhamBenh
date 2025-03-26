using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Repositories
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patients>> GetAllPatients();
        Task<Patients> GetPatientById(int id);
        Task AddPatient(Patients patient);
        Task UpdatePatient(Patients patient);
        Task DeletePatient(int id);
    }
}
