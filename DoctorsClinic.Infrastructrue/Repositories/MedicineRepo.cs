using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class MedicineRepo : RepositoryBase<Medicine, int, AppDbContext>, IMedicineRepo
    {
        public MedicineRepo(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}