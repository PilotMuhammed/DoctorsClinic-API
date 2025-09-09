using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructrue.IRepositories;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.Repositories;

namespace DoctorsClinic.Infrastructrue.Repositories
{
    public class PermissionsRepo : RepositoryBase<UserPermission, int, AppDbContext>, IPermissionsRepo
    {
        public PermissionsRepo(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}