using Microsoft.EntityFrameworkCore;
using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Repositories
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        private readonly ApplicationDbContext _context;

        public SpecialtyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Specialty>> GetAllSpecialties()
        {
            return await _context.Specialtys.ToListAsync();
        }

        public async Task<Specialty> GetSpecialtyById(int id)
        {
            return await _context.Specialtys.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddSpecialty(Specialty specialty)
        {
            await _context.Specialtys.AddAsync(specialty);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSpecialty(Specialty specialty)
        {
            _context.Specialtys.Update(specialty);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSpecialty(int id)
        {
            var specialty = await _context.Specialtys.FindAsync(id);
            if (specialty != null)
            {
                _context.Specialtys.Remove(specialty);
                await _context.SaveChangesAsync();
            }
        }
    }
}
