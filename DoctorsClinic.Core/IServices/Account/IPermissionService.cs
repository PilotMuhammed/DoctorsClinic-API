using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Permission;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices.Account
{
    public interface IPermissionService : IScopedService
    {
        Task<ResponseDto<List<GetEnum>>> Set(Guid id, List<int> pers);
        Task<ResponseDto<List<GetEnum>>> GetByUserId(Guid id);
        List<GetEnum> GetAll();
    }
}
