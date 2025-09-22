using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Account;
using DoctorsClinic.Core.Dtos.Users;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices.Account;
using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Domain.Enums;
using DoctorsClinic.Domain.Helper;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DoctorsClinic.Core.Services.Account
{
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IUserAccessorService _userAccessor;
        public UserService(IRepositoryWrapper wrapper, IUserAccessorService userAccessor)
        {
            _wrapper = wrapper;
            _userAccessor = userAccessor;
        }

        public async Task<ResponseDto<PaginationDto<UserResponseDto>>> GetAll(PaginationQuery paginationQuery, UserFilterDto filter)
        {
            #region Apply Filter
            var query = _wrapper.UserRepo.GetAll()
                .Include(i => i.Role)
                .Include(i => i.Permissions)
                .Include(i => i.AccountStatus)
                .Where(i => string.IsNullOrEmpty(filter.FullName) || i.FullName.ToLower().Contains(filter.FullName.ToLower()))
                .Where(i => string.IsNullOrEmpty(filter.UserName) || i.UserName.ToLower().Contains(filter.UserName.ToLower()))
                .Where(i => !filter.Gender.HasValue || i.Gender == filter.Gender);
            #endregion

            var data = await query
                .OrderByDescending(d => d.CreatedAt)
                .ApplyPagging(paginationQuery)
                .ProjectToType<UserResponseDto>()
                .ToListAsync();

            var count = await query.CountAsync();
            var metadata = new PaginationMetadata(count, paginationQuery);

            return new ResponseDto<PaginationDto<UserResponseDto>>(
                new PaginationDto<UserResponseDto>(data, metadata));
        }

        public async Task<ResponseDto<IEnumerable<ListDto<Guid>>>> GetList()
        {
            var query = _wrapper.UserRepo.GetAll();

            return new ResponseDto<IEnumerable<ListDto<Guid>>>(
                await query.OrderBy(d => d.FullName)
                .Select(s => new ListDto<Guid>
                {
                    Id = s.Id,
                    Title = s.FullName
                }).ToListAsync()
            );
        }

        public async Task<ResponseDto<UserResponseDto>> GetById(Guid id)
        {
            var user = await _wrapper.UserRepo.FindByCondition(u => u.Id == id)
                .Include(i => i.Role)
                .Include(i => i.Permissions)
                .Include(i => i.AccountStatus)
                .FirstOrDefaultAsync();

            if (user == null)
                return new ResponseDto<UserResponseDto>(MsgResponce.User.NotFound, true);

            return new ResponseDto<UserResponseDto>(user.Adapt<UserResponseDto>());
        }

        public async Task<ResponseDto<UserDto>> GetProfile()
        {
            var user = await _wrapper.UserRepo.FindItemByCondition(u => u.Id == _userAccessor.UserId);
            if (user == null)
                return new ResponseDto<UserDto>(MsgResponce.User.NotFound, true);

            return new ResponseDto<UserDto>(user.Adapt<UserDto>());
        }

        public async Task<ResponseDto<UserResponseDto>> Add(CreateUserDto form)
        {
            var existingUser = await _wrapper.UserRepo.FindItemByCondition(u => u.UserName.ToUpper() == form.UserName.ToUpper());
            if (existingUser != null)
                return new ResponseDto<UserResponseDto>(MsgResponce.User.UserNameExists, true);

            var role = await _wrapper.RolesRepo.FindItemByCondition(r => r.Id == form.UserRoleID);
            if (role == null && form.UserRoleID != null)
                return new ResponseDto<UserResponseDto>(MsgResponce.Role.NotFound(form.UserRoleID), true);

            var pers = form.Permissions ?? new List<int>();
            var invalidPermissions = pers.Where(p => !Enum.IsDefined(typeof(EPermission), p)).ToList();
            if (invalidPermissions.Any())
                return new ResponseDto<UserResponseDto>(MsgResponce.Permission.Invalid(invalidPermissions), true);

            var ePermissions = pers.Select(p => (EPermission)p).ToList();
            var existingPermissions = role?.Permissions?.ToList() ?? new List<EPermission>();
            var permissionsForUser = ePermissions.Where(p => !existingPermissions.Contains(p)).ToList();

            var user = form.Adapt<User>();
            user.UserRoleID = form.UserRoleID ?? 2;
            user.CreatorId = _userAccessor.UserId;
            user.Password = PasswordHasher.HashPassword(form.Password);

            user.Permissions = permissionsForUser.Select(p => new UserPermission { Permission = p }).ToList();

            await _wrapper.UserRepo.Insert(user);
            await _wrapper.SaveAllAsync();
            return new ResponseDto<UserResponseDto>(user.Adapt<UserResponseDto>());
        }

        public async Task<ResponseDto<UserResponseDto>> Update(Guid id, UpdateUserDto form)
        {
            var user = await _wrapper.UserRepo.FindByCondition(u => u.Id == id)
                .Include(i => i.Role)
                .Include(i => i.Permissions)
                .Include(i => i.AccountStatus)
                .FirstOrDefaultAsync();
            if (user == null)
                return new ResponseDto<UserResponseDto>(MsgResponce.User.NotFound, true);

            if (user.UserName != form.UserName)
            {
                var existingUser = await _wrapper.UserRepo.FindItemByCondition(u => u.UserName == form.UserName);
                if (existingUser != null)
                    return new ResponseDto<UserResponseDto>(MsgResponce.User.UserNameExists, true);
            }

            var saveUser = form.Adapt(user);
            saveUser.ModifierId = _userAccessor.UserId;
            saveUser.ModifieAt = DateTime.Now;

            await _wrapper.UserRepo.Update(saveUser);
            return new ResponseDto<UserResponseDto>(saveUser.Adapt<UserResponseDto>());
        }

        public async Task<ResponseDto<bool>> Delete(Guid id)
        {
            var user = await _wrapper.UserRepo.FindItemByCondition(u => u.Id == id);
            if (user == null)
                return new ResponseDto<bool>(MsgResponce.User.NotFound, true);

            user.DeleterId = _userAccessor.UserId;
            await _wrapper.UserRepo.Delete(id);
            await _wrapper.SaveAllAsync();
            return new ResponseDto<bool>(true);
        }

        public async Task<ResponseDto<bool>> ChangePassword(Guid id, ChangePassword form)
        {
            var data = await _wrapper.UserRepo.FindItemByCondition(s => s.Id == id);
            if (data == null)
                return new ResponseDto<bool>(MsgResponce.User.NotFound, true);

            if (!PasswordHasher.CheckPassword(data.Password, form.OldPassword))
                return new ResponseDto<bool>(MsgResponce.Password.Incorrect, true);

            if (PasswordHasher.CheckPassword(data.Password, form.NewPassword))
                return new ResponseDto<bool>(MsgResponce.Password.Same, true);

            data.Password = PasswordHasher.HashPassword(form.NewPassword);

            await _wrapper.UserRepo.Update(data);
            await _wrapper.SaveAllAsync();
            return new ResponseDto<bool>(true);
        }

        public async Task<ResponseDto<bool>> ManageAccount(Guid id, bool isBlock)
        {
            var user = await _wrapper.UserRepo.FindByCondition(u => u.Id == id)
                .Include(x => x.AccountStatus).FirstOrDefaultAsync();
            if (user == null)
                return new ResponseDto<bool>(MsgResponce.User.NotFound, true);

            user.AccountStatus!.IsBlocked = isBlock;
            user.ModifierId = _userAccessor.UserId;
            await _wrapper.UserRepo.Update(user);
            await _wrapper.SaveAllAsync();

            var message = isBlock ? "User Blocked" : "User Unblocked";
            return new ResponseDto<bool>(message);
        }

        public async Task<ResponseDto<bool>> SetActive(Guid id, bool isActive)
        {
            var user = await _wrapper.UserRepo.FindByCondition(u => u.Id == id)
                .Include(x => x.AccountStatus).FirstOrDefaultAsync();
            if (user == null)
                return new ResponseDto<bool>(MsgResponce.User.NotFound, true);

            user.AccountStatus!.IsActive = isActive;
            await _wrapper.UserRepo.Update(user);
            await _wrapper.SaveAllAsync();
            var message = isActive ? "User Activated" : "User Deactivated";
            return new ResponseDto<bool>(message);
        }

        public async Task<ResponseDto<UsersCounter>> GetCounter()
        {
            var allUsersCount = await _wrapper.UserRepo.GetAll().CountAsync();

            var activeUsers = await _wrapper.UserRepo.GetAll()
                .Include(u => u.AccountStatus)
                .Where(u => u.AccountStatus.IsActive)
                .CountAsync();

            var inactiveUsers = await _wrapper.UserRepo.GetAll()
                .Include(u => u.AccountStatus)
                .Where(u => !u.AccountStatus.IsActive && !u.IsDeleted)
                .CountAsync();

            var blockedUsers = await _wrapper.UserRepo.GetAll()
                .Include(u => u.AccountStatus)
                .Where(u => u.AccountStatus.IsBlocked && !u.IsDeleted)
                .CountAsync();

            var unBlockedUsers = await _wrapper.UserRepo.GetAll()
                .Include(u => u.AccountStatus)
                .Where(u => !u.AccountStatus.IsBlocked && !u.IsDeleted)
                .CountAsync();

            var users = new UsersCounter
            {
                UsersCount = allUsersCount,
                ActiveUsersCount = activeUsers,
                InactiveUsersCount = inactiveUsers,
                BlockedUsersCount = blockedUsers,
                UnBlockedUsersCount = unBlockedUsers
            };
            return new ResponseDto<UsersCounter>(users);
        }
    }
}