using DoctorsClinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.IRepositories
{
    public interface IPrescriptionRepo : IRepositoryBase<Prescription, int>
    {
        Task<IEnumerable<Prescription>> GetByPatientAsync(int patientId);
        Task<IEnumerable<Prescription>> GetByDoctorAsync(int doctorId);
        Task<IEnumerable<Prescription>> GetByAppointmentAsync(int appointmentId);
        Task<Prescription?> GetWithAllDetailsAsync(int prescriptionId);
        Task<IEnumerable<Prescription>> GetByPatientAndDateRangeAsync(int patientId, DateTime startDate, DateTime endDate);
    }
}
