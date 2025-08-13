using DoctorsClinic.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.IRepositories
{
    public interface IPrescriptionMedicineRepo : IRepositoryBase<PrescriptionMedicine, int>
    {
        Task<IEnumerable<PrescriptionMedicine>> GetByPrescriptionAsync(int prescriptionId);
        Task<IEnumerable<PrescriptionMedicine>> GetByMedicineAsync(int medicineId);
        Task<PrescriptionMedicine?> GetWithDetailsAsync(int id);
    }
}
