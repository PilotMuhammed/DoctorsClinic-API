using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Account;
using DoctorsClinic.Core.Dtos.Users;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices.Account;
using DoctorsClinic.Domain.Enums;
using DoctorsClinic.Domain.Helper;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DoctorsClinic.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _repo;

        public UserService(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        // GetAllAsync
        public async Task<ResponseDto<PaginationDto<UserDto>>> GetAllAsync(
            PaginationQuery pagination,
            UserFilterDto filter,
            CancellationToken ct = default)
        {
            if (pagination == null)
                return new ResponseDto<PaginationDto<UserDto>>(MsgResponce.Failed, true);

            var query = _repo.Users.GetAll(include: q => q.Include(u => u.Doctor!), track: false);

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Username))
                    query = query.Where(u => u.Username.Contains(filter.Username));
                if (!string.IsNullOrWhiteSpace(filter.Role))
                {
                    if (Enum.TryParse<UserRole>(filter.Role, true, out var roleEnum))
                        query = query.Where(u => u.Role == roleEnum);
                }
                if (filter.DoctorID.HasValue)
                    query = query.Where(u => u.DoctorID == filter.DoctorID.Value);
            }

            var totalCount = await query.CountAsync(ct);
            var data = await query
                .ApplyPagging(pagination)
                .ProjectToType<UserDto>()
                .ToListAsync(ct);

            var meta = new PaginationMetadata(totalCount, pagination);
            var paginated = new PaginationDto<UserDto>(data, meta);

            if (!data.Any())
                return new ResponseDto<PaginationDto<UserDto>>(MsgResponce.Failed, true);

            return new ResponseDto<PaginationDto<UserDto>>(paginated);
        }

        // GetListAsync
        public async Task<ResponseDto<IEnumerable<ListDto<int>>>> GetListAsync(CancellationToken ct = default)
        {
            var ids = await _repo.Users.GetAll(track: false)
                .Select(u => u.UserID)
                .ToListAsync(ct);

            if (!ids.Any())
                return new ResponseDto<IEnumerable<ListDto<int>>>(MsgResponce.Failed, true);

            var listDto = new ListDto<int>
            {
                Items = ids,
                TotalCount = ids.Count
            };

            return new ResponseDto<IEnumerable<ListDto<int>>>(new List<ListDto<int>> { listDto });
        }

        // GetByIdAsync
        public async Task<ResponseDto<UserDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<UserDto>(MsgResponce.Failed, true);

            var user = await _repo.Users.GetWithDoctorAsync(id);
            if (user == null)
                return new ResponseDto<UserDto>(MsgResponce.Failed, true);

            var dto = user.Adapt<UserDto>();
            return new ResponseDto<UserDto>(dto);
        }

        // GetProfileAsync
        public async Task<ResponseDto<UserResponseDto>> GetProfileAsync(CancellationToken ct = default)
        {
            var userId = 1;
            var user = await _repo.Users.GetWithDoctorAsync(userId);
            if (user == null)
                return new ResponseDto<UserResponseDto>(MsgResponce.Failed, true);

            var userDto = user.Adapt<UserDto>();
            var doctorDto = user.Doctor?.Adapt<Core.Dtos.Doctors.DoctorDto>();

            var response = new UserResponseDto
            {
                User = userDto,
                Doctor = doctorDto
            };

            return new ResponseDto<UserResponseDto>(response);
        }

        // CreateAsync 
        public async Task<ResponseDto<UserDto>> CreateAsync(CreateUserDto dto, CancellationToken ct = default)
        {
            if (dto == null)
                return new ResponseDto<UserDto>(MsgResponce.Failed, true);

            if (string.IsNullOrWhiteSpace(dto.Username))
                return new ResponseDto<UserDto>("Username is required.", true);

            if (string.IsNullOrWhiteSpace(dto.Password))
                return new ResponseDto<UserDto>("Password is required.", true);

            if (string.IsNullOrWhiteSpace(dto.Role))
                return new ResponseDto<UserDto>("Role is required.", true);

            var exists = await _repo.Users.UsernameExistsAsync(dto.Username);
            if (exists)
                return new ResponseDto<UserDto>("Username already exists.", true);


            if (dto.DoctorID > 0 && dto.Role.ToLower() == "doctor")
            {
                var doctor = await _repo.Doctors.GetByIdAsync(dto.DoctorID, track: false);
                if (doctor == null)
                    return new ResponseDto<UserDto>("Doctor not found.", true);
            }

            var user = dto.Adapt<Domain.Entities.User>();
            if (Enum.TryParse<UserRole>(dto.Role, true, out var roleEnum))
                user.Role = roleEnum;
            user.PasswordHash = PasswordHasher.HashPassword(dto.Password);

            await _repo.Users.AddAsync(user);
            await _repo.SaveChangesAsync();

            var resultDto = user.Adapt<UserDto>();
            return new ResponseDto<UserDto>(resultDto);
        }

        // UpdateAsync
        public async Task<ResponseDto<UserDto>> UpdateAsync(int id, UpdateUserDto dto, CancellationToken ct = default)
        {
            if (id <= 0 || dto == null)
                return new ResponseDto<UserDto>(MsgResponce.Failed, true);

            var user = await _repo.Users.GetByIdAsync(id, track: true);
            if (user == null)
                return new ResponseDto<UserDto>(MsgResponce.Failed, true);

            if (!string.IsNullOrWhiteSpace(dto.Username) && dto.Username != user.Username)
            {
                var exists = await _repo.Users.UsernameExistsAsync(dto.Username);
                if (exists)
                    return new ResponseDto<UserDto>("Username already exists.", true);

                user.Username = dto.Username;
            }

            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.PasswordHash = PasswordHasher.HashPassword(dto.Password);

            if (!string.IsNullOrWhiteSpace(dto.Role))
            {
                if (Enum.TryParse<UserRole>(dto.Role, true, out var roleEnum))
                    user.Role = roleEnum;
            }

            if (dto.DoctorID.HasValue)
                user.DoctorID = dto.DoctorID.Value;

            _repo.Users.Update(user);
            await _repo.SaveChangesAsync();

            var resultDto = user.Adapt<UserDto>();
            return new ResponseDto<UserDto>(resultDto);
        }

        // DeleteAsync
        public async Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<bool>(MsgResponce.Failed, true);

            var user = await _repo.Users.GetByIdAsync(id, track: true);
            if (user == null)
                return new ResponseDto<bool>(MsgResponce.Failed, true);

            _repo.Users.Remove(user);
            await _repo.SaveChangesAsync();

            return new ResponseDto<bool>(true);
        }

        // ChangePasswordAsync
        public async Task<ResponseDto<bool>> ChangePasswordAsync(int id, ChangePassword dto, CancellationToken ct = default)
        {
            if (id <= 0 || dto == null)
                return new ResponseDto<bool>(MsgResponce.Failed, true);

            var user = await _repo.Users.GetByIdAsync(id, track: true);
            if (user == null)
                return new ResponseDto<bool>(MsgResponce.Failed, true);

            if (!PasswordHasher.CheckPassword(user.PasswordHash, dto.OldPassword))
                return new ResponseDto<bool>("Old password is incorrect.", true);

            if (string.IsNullOrWhiteSpace(dto.NewPassword))
                return new ResponseDto<bool>("New password is required.", true);

            user.PasswordHash = PasswordHasher.HashPassword(dto.NewPassword);

            _repo.Users.Update(user);
            await _repo.SaveChangesAsync();

            return new ResponseDto<bool>(true);
        }

        // ManageAccountAsync
        public async Task<ResponseDto<bool>> ManageAccountAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<bool>(MsgResponce.Failed, true);

            var user = await _repo.Users.GetByIdAsync(id, track: true);
            if (user == null)
                return new ResponseDto<bool>(MsgResponce.Failed, true);

            _repo.Users.Update(user);
            await _repo.SaveChangesAsync();

            return new ResponseDto<bool>(true);
        }

        // SetActiveAsync
        public async Task<ResponseDto<bool>> SetActiveAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return new ResponseDto<bool>(MsgResponce.User.NotFound, true);

            var user = await _repo.Users.GetByIdAsync(id, track: true);
            if (user == null)
                return new ResponseDto<bool>(MsgResponce.User.NotFound, true);

            _repo.Users.Update(user);
            await _repo.SaveChangesAsync();

            return new ResponseDto<bool>(true);
        }

        // GetCounterAsync
        public async Task<ResponseDto<UsersCounter>> GetCounterAsync(CancellationToken ct = default)
        {
            var users = await _repo.Users.GetAll(track: false).ToListAsync(ct);

            var result = new UsersCounter
            {
                DoctorsCount = users.Count(u => u.Role == UserRole.Doctor),
                ReceptionistsCount = users.Count(u => u.Role == UserRole.Receptionist),
                NursesCount = users.Count(u => u.Role == UserRole.Nurse),
                UsersCount = users.Count
            };

            return new ResponseDto<UsersCounter>(result);
        }
    }
}
