using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Repositories
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctors>> GetAllDoctors();
        Task<Doctors> GetDoctorById(int id);
        Task AddDoctor(Doctors doctor);
        Task UpdateDoctor(Doctors doctor);
        Task DeleteDoctor(int id);
    }
}
