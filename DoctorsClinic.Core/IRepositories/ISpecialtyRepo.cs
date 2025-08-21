using DoctorsClinic.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.IRepositories
{
    public interface ISpecialtyRepo : IRepositoryBase<Specialty, int>
    {
        Task<IEnumerable<Specialty>> SearchByNameAsync(string partialName);
        Task<Specialty?> GetWithDoctorsAsync(int specialtyId);
        Task<IEnumerable<Specialty>> GetAllWithDoctorsAsync();
    }
}
