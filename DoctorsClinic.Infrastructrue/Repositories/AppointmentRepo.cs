using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class AppointmentRepo : RepositoryBase<Appointment, int>, IAppointmentRepo
    {
        public AppointmentRepo(DbContext context) : base(context) { }

        public override async Task<Appointment?> GetByIdAsync(
            int appointmentId,
            Func<IQueryable<Appointment>, IIncludableQueryable<Appointment, object>>? include = null,
            bool track = false)
        {
            IQueryable<Appointment> query = _dbSet;

            if (!track)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(a => a.AppointmentID == appointmentId);
        }
        public async Task<IEnumerable<Appointment>> GetByPatientAsync(int patientId)
        {
            return await _dbSet
                .Where(a => a.PatientID == patientId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Appointment>> GetByDoctorAsync(int doctorId)
        {
            return await _dbSet
                .Where(a => a.DoctorID == doctorId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Appointment>> GetUpcomingByDoctorAsync(int doctorId, DateTime fromDate)
        {
            return await _dbSet
                .Where(a => a.DoctorID == doctorId && a.AppointmentDate >= fromDate)
                .ToListAsync();
        }
        public async Task<Appointment?> GetWithAllDetailsAsync(int appointmentId)
        {
            return await _dbSet
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Prescriptions)
                .Include(a => a.Invoice)
                .FirstOrDefaultAsync(a => a.AppointmentID == appointmentId);
        }
    }
}
