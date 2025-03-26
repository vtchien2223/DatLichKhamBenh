using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Repositories
{
    public interface ISpecialtyRepository
    {
        Task<IEnumerable<Specialty>> GetAllSpecialties();
        Task<Specialty> GetSpecialtyById(int id);
        Task AddSpecialty(Specialty specialty);
        Task UpdateSpecialty(Specialty specialty);
        Task DeleteSpecialty(int id);
    }
}
