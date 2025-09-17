using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Role;
using DoctorsClinic.Core.Dtos.Users;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices.Account
{
    public interface IRoleService : IScopedService
    {
        Task<ResponseDto<PaginationDto<GetRole>>> GetAll(PaginationQuery paginationQuery, RoleFilter filter);
        Task<ResponseDto<IEnumerable<ListDto<int>>>> GetList();
        Task<ResponseDto<GetRole>> GetById(int id);
        Task<ResponseDto<GetRole>> Add(CreateRole form);
        Task<ResponseDto<GetRole>> Update(int id, UpdateRole form);
        Task<ResponseDto<bool>> Delete(int id);
        Task<ResponseDto<UserResponseDto>> SetRoleToUser(int userId, int roleId);
    }
}
