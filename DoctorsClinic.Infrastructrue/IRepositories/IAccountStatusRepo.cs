using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.IRepositories;

namespace DoctorsClinic.Infrastructrue.IRepositories
{
    public interface IAccountStatusRepo : IRepositoryBase<AccountStatus, int>
    {
    }
}