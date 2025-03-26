using Microsoft.EntityFrameworkCore;
using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Services>> GetAllServices()
        {
            return await _context.Services.ToListAsync();
        }

        public async Task<Services> GetServiceById(int id)
        {
            return await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddService(Services service)
        {
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateService(Services service)
        {
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }
        }
    }
}
