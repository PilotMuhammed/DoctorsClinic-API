using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class PaymentRepo : RepositoryBase<Payment, int>, IPaymentRepo
    {
        public PaymentRepo(DbContext context) : base(context) { }

        public override async Task<Payment?> GetByIdAsync(
            int paymentId,
            Func<IQueryable<Payment>, IIncludableQueryable<Payment, object>>? include = null,
            bool track = false)
        {
            IQueryable<Payment> query = _dbSet;

            if (!track)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(p => p.PaymentID == paymentId);
        }
        public async Task<IEnumerable<Payment>> GetByInvoiceAsync(int invoiceId)
        {
            return await _dbSet
                .Where(p => p.InvoiceID == invoiceId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByPatientAsync(int patientId)
        {
            return await _dbSet
                .Include(p => p.Invoice)
                .Where(p => p.Invoice.PatientID == patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(p => p.Date >= startDate && p.Date <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByDateAsync(DateTime date)
        {
            var dateOnly = date.Date;
            return await _dbSet
                .Where(p => p.Date.Date == dateOnly)
                .ToListAsync();
        }

        public async Task<Payment?> GetWithInvoiceAsync(int paymentId)
        {
            return await _dbSet
                .Include(p => p.Invoice)
                .FirstOrDefaultAsync(p => p.PaymentID == paymentId);
        }

        public async Task<decimal> GetTotalAmountByInvoiceAsync(int invoiceId)
        {
            return await _dbSet
                .Where(p => p.InvoiceID == invoiceId)
                .SumAsync(p => p.Amount);
        }
    }
}
