using DoctorsClinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.IRepositories
{
    public interface IInvoiceRepo : IRepositoryBase<Invoice, int>
    {
        Task<IEnumerable<Invoice>> GetByPatientAsync(int patientId);

        Task<Invoice?> GetWithAllDetailsAsync(int invoiceId);
        Task<IEnumerable<Invoice>> GetByPatientAndDateRangeAsync(int patientId, DateTime startDate, DateTime endDate);
        Task<Invoice?> GetByAppointmentIdAsync(int appointmentId);
        Task<IEnumerable<Invoice>> GetByDateAsync(DateTime date);
    }
}
