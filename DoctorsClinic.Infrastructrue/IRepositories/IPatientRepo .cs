using DoctorsClinic.Domain.Entities;

namespace DoctorsClinic.Infrastructure.IRepositories
{
    public interface IPatientRepo : IRepositoryBase<Patient, int>
    {
    }
}
