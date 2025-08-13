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
    public class InvoiceRepo : RepositoryBase<Invoice, int>, IInvoiceRepo
    {
        public InvoiceRepo(DbContext context) : base(context) { }

        public override async Task<Invoice?> GetByIdAsync(
            int invoiceId,
            Func<IQueryable<Invoice>, IIncludableQueryable<Invoice, object>>? include = null,
            bool track = false)
        {
            IQueryable<Invoice> query = _dbSet;

            if (!track)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(i => i.InvoiceID == invoiceId);
        }
        public async Task<IEnumerable<Invoice>> GetByPatientAsync(int patientId)
        {
            return await _dbSet
                .Where(i => i.PatientID == patientId)
                .ToListAsync();
        }
        public async Task<Invoice?> GetWithAllDetailsAsync(int invoiceId)
        {
            return await _dbSet
                .Include(i => i.Patient)
                .Include(i => i.Appointment)
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(i => i.InvoiceID == invoiceId);
        }
        public async Task<IEnumerable<Invoice>> GetByPatientAndDateRangeAsync(int patientId, DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(i => i.PatientID == patientId && i.Date >= startDate && i.Date <= endDate)
                .ToListAsync();
        }
        public async Task<Invoice?> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(i => i.AppointmentID == appointmentId);
        }
        public async Task<IEnumerable<Invoice>> GetByDateAsync(DateTime date)
        {
            var dateOnly = date.Date;
            return await _dbSet
                .Where(i => i.Date.Date == dateOnly)
                .ToListAsync();
        }
    }
}
