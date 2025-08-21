using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class DoctorRepo : RepositoryBase<Doctor, int>, IDoctorRepo
    {
        public DoctorRepo(AppDbContext context) : base(context) { }

        public override async Task<Doctor?> GetByIdAsync(
            int doctorId,
            Func<IQueryable<Doctor>, IIncludableQueryable<Doctor, object>>? include = null,
            bool track = false)
        {
            IQueryable<Doctor> query = _dbSet;

            if (!track)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(d => d.DoctorID == doctorId);
        }

        public async Task<IEnumerable<Doctor>> SearchByNameAsync(string partialName)
        {
            return await _dbSet
                .Where(d => d.FullName.Contains(partialName))
                .ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetBySpecialtyAsync(int specialtyId, bool includeAppointments = false)
        {
            IQueryable<Doctor> query = _dbSet.Where(d => d.SpecialtyID == specialtyId);
            if (includeAppointments)
                query = query.Include(d => d.Appointments);
            return await query.ToListAsync();
        }

        public async Task<Doctor?> GetWithAllDetailsAsync(int doctorId)
        {
            return await _dbSet
                .Include(d => d.Specialty)
                .Include(d => d.User)
                .Include(d => d.Appointments)
                .Include(d => d.MedicalRecords)
                .Include(d => d.Prescriptions)
                .FirstOrDefaultAsync(d => d.DoctorID == doctorId);
        }
    }
}
