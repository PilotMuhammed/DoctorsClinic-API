using DoctorsClinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.IRepositories
{
    public interface IMedicalRecordRepo : IRepositoryBase<MedicalRecord, int>
    {
        Task<IEnumerable<MedicalRecord>> GetByPatientAsync(int patientId);
        Task<IEnumerable<MedicalRecord>> GetByDoctorAsync(int doctorId);
        Task<IEnumerable<MedicalRecord>> GetByPatientAndDateRangeAsync(int patientId, DateTime startDate, DateTime endDate);
        Task<MedicalRecord?> GetWithDetailsAsync(int recordId);
    }
}
