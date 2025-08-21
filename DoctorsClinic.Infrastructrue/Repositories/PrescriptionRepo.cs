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
    public class PrescriptionRepo : RepositoryBase<Prescription, int>, IPrescriptionRepo
    {
        public PrescriptionRepo(AppDbContext context) : base(context) { }

        public override async Task<Prescription?> GetByIdAsync(
            int prescriptionId,
            Func<IQueryable<Prescription>, IIncludableQueryable<Prescription, object>>? include = null,
            bool track = false)
        {
            IQueryable<Prescription> query = _dbSet;

            if (!track)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(p => p.PrescriptionID == prescriptionId);
        }
        public async Task<IEnumerable<Prescription>> GetByPatientAsync(int patientId)
        {
            return await _dbSet
                .Where(p => p.PatientID == patientId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Prescription>> GetByDoctorAsync(int doctorId)
        {
            return await _dbSet
                .Where(p => p.DoctorID == doctorId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Prescription>> GetByAppointmentAsync(int appointmentId)
        {
            return await _dbSet
                .Where(p => p.AppointmentID == appointmentId)
                .ToListAsync();
        }
        public async Task<Prescription?> GetWithAllDetailsAsync(int prescriptionId)
        {
            return await _dbSet
                .Include(p => p.Appointment!)
                .Include(p => p.Doctor!)
                .Include(p => p.Patient!)
                .Include(p => p.PrescriptionMedicines!)
                    .ThenInclude(pm => pm.Medicine)
                .FirstOrDefaultAsync(p => p.PrescriptionID == prescriptionId);
        }
        public async Task<IEnumerable<Prescription>> GetByPatientAndDateRangeAsync(int patientId, DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(p => p.PatientID == patientId && p.Date >= startDate && p.Date <= endDate)
                .ToListAsync();
        }
    }
}
