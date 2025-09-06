using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class PatientRepo : RepositoryBase<Patient, int, AppDbContext>, IPatientRepo
    {
        public PatientRepo(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}