using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructrue.IRepositories;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.Repositories;

namespace DoctorsClinic.Infrastructrue.Repositories
{
    public class AccountStatusRepo : RepositoryBase<AccountStatus, int, AppDbContext>, IAccountStatusRepo
    {
        public AccountStatusRepo(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}