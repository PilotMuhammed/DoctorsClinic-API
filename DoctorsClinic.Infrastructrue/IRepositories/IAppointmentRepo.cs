using DoctorsClinic.Domain.Entities;

namespace DoctorsClinic.Infrastructure.IRepositories
{
    public interface IAppointmentRepo : IRepositoryBase<Appointment, int>
    {
    }
}
