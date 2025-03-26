using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Repositories
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Services>> GetAllServices();
        Task<Services> GetServiceById(int id);
        Task AddService(Services service);
        Task UpdateService(Services service);
        Task DeleteService(int id);
    }
}
