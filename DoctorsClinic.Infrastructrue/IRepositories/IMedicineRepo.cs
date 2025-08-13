using DoctorsClinic.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.IRepositories
{

    public interface IMedicineRepo : IRepositoryBase<Medicine, int>
    {
        Task<IEnumerable<Medicine>> SearchByNameOrTypeAsync(string query);
        Task<Medicine?> GetWithPrescriptionsAsync(int medicineId);
        Task<IEnumerable<Medicine>> GetByPrescriptionIdAsync(int prescriptionId);
    }
}
