using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Account;
using DoctorsClinic.Core.Dtos.Users;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;


namespace DoctorsClinic.Core.IServices
{
    public interface IUserService : IScopedService
    {
        Task<ResponseDto<PaginationDto<UserDto>>> GetAllAsync(
            PaginationQuery pagination,
            UserFilterDto filter,
            CancellationToken ct = default);

        Task<ResponseDto<IEnumerable<ListDto<int>>>> GetListAsync(CancellationToken ct = default);

        Task<ResponseDto<UserDto>> GetByIdAsync(int id, CancellationToken ct = default);

        Task<ResponseDto<UserResponseDto>> GetProfileAsync(CancellationToken ct = default);

        Task<ResponseDto<UserDto>> CreateAsync(CreateUserDto dto, CancellationToken ct = default);

        Task<ResponseDto<UserDto>> UpdateAsync(int id, UpdateUserDto dto, CancellationToken ct = default);

        Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default);

        Task<ResponseDto<bool>> ChangePasswordAsync(int id, ChangePassword dto, CancellationToken ct = default);

        Task<ResponseDto<bool>> ManageAccountAsync(int id, bool isBlocked, CancellationToken ct = default);

        Task<ResponseDto<bool>> SetActiveAsync(int id, bool isActive, CancellationToken ct = default);

        Task<ResponseDto<UsersCounter>> GetCounterAsync(CancellationToken ct = default);
    }
}