using Microsoft.EntityFrameworkCore;
using Nhom4_QLBA_API.Models;

namespace Nhom4_QLBA_API.Repositories
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public MedicalRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MedicalRecordDetails>> GetAllMedicalRecords()
        {
            return await _context.MedicalRecordDetails
                .Include(m => m.Patient)
                .ToListAsync();
        }

        public async Task<MedicalRecordDetails> GetMedicalRecordById(int id)
        {
            return await _context.MedicalRecordDetails
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddMedicalRecord(MedicalRecordDetails medicalRecord)
        {
            await _context.MedicalRecordDetails.AddAsync(medicalRecord);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMedicalRecord(MedicalRecordDetails medicalRecord)
        {
            _context.MedicalRecordDetails.Update(medicalRecord);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMedicalRecord(int id)
        {
            var medicalRecord = await _context.MedicalRecordDetails.FindAsync(id);
            if (medicalRecord != null)
            {
                _context.MedicalRecordDetails.Remove(medicalRecord);
                await _context.SaveChangesAsync();
            }
        }
    }
}
