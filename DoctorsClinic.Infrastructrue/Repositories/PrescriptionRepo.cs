using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class PrescriptionRepo : RepositoryBase<Prescription, int, AppDbContext>, IPrescriptionRepo
    {
        public PrescriptionRepo(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}