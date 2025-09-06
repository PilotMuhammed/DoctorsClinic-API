using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class MedicalRecordRepo : RepositoryBase<MedicalRecord, int, AppDbContext>, IMedicalRecordRepo
    {
        public MedicalRecordRepo(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}