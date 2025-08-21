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
    public class UserRepo : RepositoryBase<User, int>, IUserRepo
    {
        public UserRepo(AppDbContext context) : base(context) { }

        public override async Task<User?> GetByIdAsync(
            int userId,
            Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null,
            bool track = false)
        {
            IQueryable<User> query = _dbSet;

            if (!track)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(u => u.UserID == userId);
        }
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<User?> GetByUsernameAndPasswordAsync(string username, string passwordHash)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == passwordHash);
        }
        public async Task<IEnumerable<User>> GetByRoleAsync(Domain.Enums.UserRole role)
        {
            return await _dbSet.Where(u => u.Role == role).ToListAsync();
        }
        public async Task<User?> GetWithDoctorAsync(int userId)
        {
            return await _dbSet
                .Include(u => u.Doctor)
                .FirstOrDefaultAsync(u => u.UserID == userId);
        }
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _dbSet.AnyAsync(u => u.Username == username);
        }
    }
}
