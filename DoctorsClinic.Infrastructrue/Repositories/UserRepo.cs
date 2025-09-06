using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class UserRepo : RepositoryBase<User, int, AppDbContext>, IUserRepo
    {
        public UserRepo(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}