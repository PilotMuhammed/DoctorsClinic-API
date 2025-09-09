using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.IRepositories;

namespace DoctorsClinic.Infrastructrue.IRepositories
{
    public interface IRolesRepo : IRepositoryBase<UserRole, int>
    {
    }
}