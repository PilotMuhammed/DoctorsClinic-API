using DoctorsClinic.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.IRepositories
{
    public interface IDoctorRepo : IRepositoryBase<Doctor, int>
    {
        Task<IEnumerable<Doctor>> SearchByNameAsync(string partialName);
        Task<IEnumerable<Doctor>> GetBySpecialtyAsync(int specialtyId, bool includeAppointments = false);
        Task<Doctor?> GetWithAllDetailsAsync(int doctorId);
    }
}
