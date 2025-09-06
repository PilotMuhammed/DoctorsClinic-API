using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class InvoiceRepo : RepositoryBase<Invoice, int, AppDbContext>, IInvoiceRepo
    {
        public InvoiceRepo(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}