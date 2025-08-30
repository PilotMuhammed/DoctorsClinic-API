using DoctorsClinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.IRepositories
{
    public interface IAppointmentRepo : IRepositoryBase<Appointment, int>
    {
        Task<IEnumerable<Appointment>> GetByPatientAsync(int patientId);
        Task<IEnumerable<Appointment>> GetByDoctorAsync(int doctorId);
        Task<IEnumerable<Appointment>> GetUpcomingByDoctorAsync(int doctorId, DateTime fromDate);
        Task<Appointment?> GetWithAllDetailsAsync(int appointmentId);
    }
}
