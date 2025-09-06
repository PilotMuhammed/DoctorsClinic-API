using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class PrescriptionMedicineRepo : RepositoryBase<PrescriptionMedicine, int, AppDbContext>, IPrescriptionMedicineRepo
    {
        public PrescriptionMedicineRepo(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}