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
        Task<ResponseDto<IEnumerable<ListDto<int>>>> GetList();
        Task<ResponseDto<UserResponseDto>> GetById(int id);
        Task<ResponseDto<UserDto>> GetProfile();
        Task<ResponseDto<UserResponseDto>> Add(CreateUserDto form);
        Task<ResponseDto<UserResponseDto>> Update(int id, UpdateUserDto form);
        Task<ResponseDto<bool>> Delete(int id);
        Task<ResponseDto<bool>> ChangePassword(int id, ChangePassword form);
        Task<ResponseDto<bool>> ManageAccount(int id, bool isBlock);
        Task<ResponseDto<bool>> SetActive(int id, bool isActive);
        Task<ResponseDto<UsersCounter>> GetCounter();
    }
}
