using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class SpecialtyRepo : RepositoryBase<Specialty, int, AppDbContext>, ISpecialtyRepo
    {
        public SpecialtyRepo(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}