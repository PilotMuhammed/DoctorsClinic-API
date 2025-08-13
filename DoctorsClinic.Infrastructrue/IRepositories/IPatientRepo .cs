using DoctorsClinic.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.IRepositories
{
    public interface IPatientRepo : IRepositoryBase<Patient, int>
    {
        Task<IEnumerable<Patient>> SearchByNameAsync(string partialName);
        Task<Patient?> GetWithAllDetailsAsync(int patientId);
    }
}
