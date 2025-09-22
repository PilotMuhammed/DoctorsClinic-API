using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Account;
using DoctorsClinic.Core.Dtos.Users;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices.Account
{
    public interface IUserService : IScopedService
    {
        Task<ResponseDto<PaginationDto<UserResponseDto>>> GetAll(PaginationQuery paginationQuery, UserFilterDto filter);
        Task<ResponseDto<IEnumerable<ListDto<Guid>>>> GetList();
        Task<ResponseDto<UserResponseDto>> GetById(Guid id);
        Task<ResponseDto<UserDto>> GetProfile();
        Task<ResponseDto<UserResponseDto>> Add(CreateUserDto form);
        Task<ResponseDto<UserResponseDto>> Update(Guid id, UpdateUserDto form);
        Task<ResponseDto<bool>> Delete(Guid id);
        Task<ResponseDto<bool>> ChangePassword(Guid id, ChangePassword form);
        Task<ResponseDto<bool>> ManageAccount(Guid id, bool isBlock);
        Task<ResponseDto<bool>> SetActive(Guid id, bool isActive);
        Task<ResponseDto<UsersCounter>> GetCounter();
    }
}
