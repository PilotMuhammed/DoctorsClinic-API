using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructrue.IRepositories;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.Repositories;

namespace DoctorsClinic.Infrastructrue.Repositories
{
    public class RolesRepo : RepositoryBase<UserRole, int, AppDbContext>, IRolesRepo
    {
        public RolesRepo(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}