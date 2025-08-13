using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class SpecialtyRepo : RepositoryBase<Specialty, int>, ISpecialtyRepo
    {
        public SpecialtyRepo(DbContext context) : base(context) { }

        public override async Task<Specialty?> GetByIdAsync(
            int specialtyId,
            Func<IQueryable<Specialty>, IIncludableQueryable<Specialty, object>>? include = null,
            bool track = false)
        {
            IQueryable<Specialty> query = _dbSet;

            if (!track)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(s => s.SpecialtyID == specialtyId);
        }
        public async Task<IEnumerable<Specialty>> SearchByNameAsync(string partialName)
        {
            return await _dbSet
                .Where(s => s.Name.Contains(partialName))
                .ToListAsync();
        }
        public async Task<Specialty?> GetWithDoctorsAsync(int specialtyId)
        {
            return await _dbSet
                .Include(s => s.Doctors)
                .FirstOrDefaultAsync(s => s.SpecialtyID == specialtyId);
        }
        public async Task<IEnumerable<Specialty>> GetAllWithDoctorsAsync()
        {
            return await _dbSet
                .Include(s => s.Doctors)
                .ToListAsync();
        }
    }
}
