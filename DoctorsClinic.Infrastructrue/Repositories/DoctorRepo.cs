using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class DoctorRepo : RepositoryBase<Doctor, int, AppDbContext>, IDoctorRepo
    {
        public DoctorRepo(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}