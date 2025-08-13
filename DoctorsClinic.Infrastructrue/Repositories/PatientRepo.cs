using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class PatientRepo : RepositoryBase<Patient, int>, IPatientRepo
    {
        public PatientRepo(DbContext context) : base(context) { }

        public override async Task<Patient?> GetByIdAsync(
            int patientId,
            Func<IQueryable<Patient>, IIncludableQueryable<Patient, object>>? include = null,
            bool track = false)
        {
            IQueryable<Patient> query = _dbSet;

            if (!track)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(p => p.PatientID == patientId);
        }

        public async Task<IEnumerable<Patient>> SearchByNameAsync(string partialName)
        {
            return await _dbSet
                .Where(p => p.FullName.Contains(partialName))
                .ToListAsync();
        }

        public async Task<Patient?> GetWithAllDetailsAsync(int patientId)
        {
            return await _dbSet
                .Include(p => p.Appointments)
                .Include(p => p.MedicalRecords)
                .Include(p => p.Prescriptions)
                .Include(p => p.Invoices)
                .FirstOrDefaultAsync(p => p.PatientID == patientId);
        }
    }
}
