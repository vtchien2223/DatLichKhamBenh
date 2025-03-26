using Microsoft.EntityFrameworkCore;
using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patients>> GetAllPatients()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task<Patients> GetPatientById(int id)
        {
            return await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddPatient(Patients patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePatient(Patients patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
        }
    }
}
