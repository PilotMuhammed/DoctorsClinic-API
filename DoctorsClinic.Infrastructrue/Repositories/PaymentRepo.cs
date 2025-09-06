using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class PaymentRepo : RepositoryBase<Payment, int, AppDbContext>, IPaymentRepo
    {
        public PaymentRepo(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}