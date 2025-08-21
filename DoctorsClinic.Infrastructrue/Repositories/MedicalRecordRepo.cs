using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class MedicalRecordRepo : RepositoryBase<MedicalRecord, int>, IMedicalRecordRepo
    {
        public MedicalRecordRepo(AppDbContext context) : base(context) { }

        public override async Task<MedicalRecord?> GetByIdAsync(
            int recordId,
            Func<IQueryable<MedicalRecord>, IIncludableQueryable<MedicalRecord, object>>? include = null,
            bool track = false)
        {
            IQueryable<MedicalRecord> query = _dbSet;

            if (!track)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(m => m.RecordID == recordId);
        }
        public async Task<IEnumerable<MedicalRecord>> GetByPatientAsync(int patientId)
        {
            return await _dbSet
                .Where(m => m.PatientID == patientId)
                .ToListAsync();
        }
        public async Task<IEnumerable<MedicalRecord>> GetByDoctorAsync(int doctorId)
        {
            return await _dbSet
                .Where(m => m.DoctorID == doctorId)
                .ToListAsync();
        }
        public async Task<IEnumerable<MedicalRecord>> GetByPatientAndDateRangeAsync(int patientId, DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(m => m.PatientID == patientId && m.Date >= startDate && m.Date <= endDate)
                .ToListAsync();
        }
        public async Task<MedicalRecord?> GetWithDetailsAsync(int recordId)
        {
            return await _dbSet
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                .FirstOrDefaultAsync(m => m.RecordID == recordId);
        }
    }
}
