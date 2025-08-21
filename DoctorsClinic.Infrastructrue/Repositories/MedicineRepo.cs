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
    public class MedicineRepo : RepositoryBase<Medicine, int>, IMedicineRepo
    {
        public MedicineRepo(AppDbContext context) : base(context) { }

        public override async Task<Medicine?> GetByIdAsync(
            int medicineId,
            Func<IQueryable<Medicine>, IIncludableQueryable<Medicine, object>>? include = null,
            bool track = false)
        {
            IQueryable<Medicine> query = _dbSet;

            if (!track)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(m => m.MedicineID == medicineId);
        }
        public async Task<IEnumerable<Medicine>> SearchByNameOrTypeAsync(string query)
        {
            return await _dbSet
                .Where(m => m.Name.Contains(query) || m.Type.Contains(query))
                .ToListAsync();
        }
        public async Task<Medicine?> GetWithPrescriptionsAsync(int medicineId)
        {
            return await _dbSet
                .Include(m => m.PrescriptionMedicines!)
                    .ThenInclude(pm => pm.Prescription)
                .FirstOrDefaultAsync(m => m.MedicineID == medicineId);
        }
        public async Task<IEnumerable<Medicine>> GetByPrescriptionIdAsync(int prescriptionId)
        {
            return await _dbSet
                .Where(m => m.PrescriptionMedicines!.Any(pm => pm.PrescriptionID == prescriptionId))
                .ToListAsync();
        }
    }
}
