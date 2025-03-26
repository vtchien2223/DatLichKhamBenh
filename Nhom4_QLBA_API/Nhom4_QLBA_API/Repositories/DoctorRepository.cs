using Microsoft.EntityFrameworkCore;
using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _context;

        public DoctorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctors>> GetAllDoctors()
        {
            return await _context.Doctors.ToListAsync();
        }

        public async Task<Doctors> GetDoctorById(int id)
        {
            return await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddDoctor(Doctors doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDoctor(Doctors doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
            }
        }
    }
}
