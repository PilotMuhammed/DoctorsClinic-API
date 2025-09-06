using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class AppointmentRepo : RepositoryBase<Appointment, int, AppDbContext>, IAppointmentRepo
    {
        public AppointmentRepo(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}