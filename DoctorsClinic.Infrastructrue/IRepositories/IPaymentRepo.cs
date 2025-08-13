using DoctorsClinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.IRepositories
{
    public interface IPaymentRepo : IRepositoryBase<Payment, int>
    {
        Task<IEnumerable<Payment>> GetByInvoiceAsync(int invoiceId);
        Task<IEnumerable<Payment>> GetByPatientAsync(int patientId);
        Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Payment>> GetByDateAsync(DateTime date);
        Task<Payment?> GetWithInvoiceAsync(int paymentId);
        Task<decimal> GetTotalAmountByInvoiceAsync(int invoiceId);
    }
}
