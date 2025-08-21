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
    public class PrescriptionMedicineRepo : RepositoryBase<PrescriptionMedicine, int>, IPrescriptionMedicineRepo
    {
        public PrescriptionMedicineRepo(AppDbContext context) : base(context) { }

        public override async Task<PrescriptionMedicine?> GetByIdAsync(
            int id,
            Func<IQueryable<PrescriptionMedicine>, IIncludableQueryable<PrescriptionMedicine, object>>? include = null,
            bool track = false)
        {
            IQueryable<PrescriptionMedicine> query = _dbSet;

            if (!track)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(pm => pm.ID == id);
        }
        public async Task<IEnumerable<PrescriptionMedicine>> GetByPrescriptionAsync(int prescriptionId)
        {
            return await _dbSet
                .Where(pm => pm.PrescriptionID == prescriptionId)
                .ToListAsync();
        }
        public async Task<IEnumerable<PrescriptionMedicine>> GetByMedicineAsync(int medicineId)
        {
            return await _dbSet
                .Where(pm => pm.MedicineID == medicineId)
                .ToListAsync();
        }
        public async Task<PrescriptionMedicine?> GetWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(pm => pm.Prescription)
                .Include(pm => pm.Medicine)
                .FirstOrDefaultAsync(pm => pm.ID == id);
        }
    }
}
