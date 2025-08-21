using DoctorsClinic.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.IRepositories
{
    public interface IUserRepo : IRepositoryBase<User, int>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByUsernameAndPasswordAsync(string username, string passwordHash);
        Task<IEnumerable<User>> GetByRoleAsync(Domain.Enums.UserRole role);
        Task<User?> GetWithDoctorAsync(int userId);
        Task<bool> UsernameExistsAsync(string username);
    }
}
