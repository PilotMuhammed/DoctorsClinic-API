using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Role;
using DoctorsClinic.Core.Dtos.Users;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices.Account;
using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Domain.Enums;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DoctorsClinic.Core.Services.Account
{
    public class RoleService : IRoleService
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IUserAccessorService _userAccessor;
        public RoleService(IRepositoryWrapper wrapper, IUserAccessorService userAccessor)
        {
            _wrapper = wrapper;
            _userAccessor = userAccessor;
        }

        public async Task<ResponseDto<PaginationDto<GetRole>>> GetAll(PaginationQuery paginationQuery, RoleFilter filter)
        {
            var query = _wrapper.RolesRepo.GetAll()
                .Where(x => string.IsNullOrWhiteSpace(filter.Name) || x.Name.Contains(filter.Name))
                .Where(x => string.IsNullOrWhiteSpace(filter.NameAr) || x.NameAr.Contains(filter.NameAr))
                .Where(x => string.IsNullOrWhiteSpace(filter.Description) || x.Description!.Contains(filter.Description));

            var data = await query
                .OrderByDescending(c => c.Id)
                .ApplyPagging(paginationQuery)
                .ProjectToType<GetRole>()
                .ToListAsync();

            var count = await query.CountAsync();
            var metadata = new PaginationMetadata(count, paginationQuery);

            return new ResponseDto<PaginationDto<GetRole>>(
                new PaginationDto<GetRole>(data, metadata));
        }

        public async Task<ResponseDto<IEnumerable<ListDto<int>>>> GetList()
        {
            var query = _wrapper.RolesRepo.GetAll();

            return new ResponseDto<IEnumerable<ListDto<int>>>(
                await query.OrderBy(d => d.Id)
                .Select(s => new ListDto<int>
                {
                    Id = s.Id,
                    Title = s.Name
                }).ToListAsync()
            );
        }

        public async Task<ResponseDto<GetRole>> GetById(int id)
        {
            var data = await _wrapper.RolesRepo.FindItemByCondition(x => x.Id == id);
            if (data == null)
                return new ResponseDto<GetRole>(MsgResponce.Role.NotFound(id), true);

            var result = data.Adapt<GetRole>();
            return new ResponseDto<GetRole>(result);
        }

        public async Task<ResponseDto<GetRole>> Add(CreateRole form)
        {
            var existingRole = await _wrapper.RolesRepo.FindItemByCondition(r => r.Name == form.Name);
            if (existingRole != null)
                return new ResponseDto<GetRole>(MsgResponce.Role.NameExists, true);

            var invalidPermissions = form.Permissions.Where(p => !Enum.IsDefined(typeof(EPermission), p)).ToList();
            if (invalidPermissions.Any())
                return new ResponseDto<GetRole>(MsgResponce.Permission.Invalid(invalidPermissions), true);

            var data = form.Adapt<UserRole>();
            data.CreatorId = _userAccessor.UserId;
            data.Permissions = form.Permissions.Select(x => (EPermission)x).Distinct().ToList();

            await _wrapper.RolesRepo.Insert(data);
            await _wrapper.SaveAllAsync();

            return new ResponseDto<GetRole>(data.Adapt<GetRole>());
        }

        public async Task<ResponseDto<GetRole>> Update(int id, UpdateRole form)
        {
            var existingRole = await _wrapper.RolesRepo.FindItemByCondition(r => r.Name == form.Name);
            if (existingRole != null)
                return new ResponseDto<GetRole>(MsgResponce.Role.NameExists, true);

            var invalidPermissions = form.Permissions!.Where(p => !Enum.IsDefined(typeof(EPermission), p)).ToList();
            if (invalidPermissions.Any())
                return new ResponseDto<GetRole>(MsgResponce.Permission.Invalid(invalidPermissions), true);

            var data = await _wrapper.RolesRepo.FindItemByCondition(x => x.Id == id);
            if (data == null)
                return new ResponseDto<GetRole>(MsgResponce.Role.NotFound(id), true);

            #region Update Data
            var saveRole = form.Adapt(data);

            var ePermissions = form.Permissions!.Select(p => (EPermission)p).ToList();
            saveRole.Permissions.Clear();
            var permissions = ePermissions.Select(x => x).ToList();
            saveRole.Permissions = permissions;

            saveRole.ModifierId = _userAccessor.UserId;
            #endregion

            await _wrapper.RolesRepo.Update(saveRole);
            return new ResponseDto<GetRole>(saveRole.Adapt<GetRole>());
        }

        public async Task<ResponseDto<bool>> Delete(int id)
        {
            var data = await _wrapper.RolesRepo.FindItemByCondition(r => r.Id == id);
            if (data == null)
                return new ResponseDto<bool>(MsgResponce.Role.NotFound(id), true);

            data.DeleterId = _userAccessor.UserId;
            await _wrapper.RolesRepo.Delete(id);
            await _wrapper.SaveAllAsync();
            return new ResponseDto<bool>(true);
        }

        public async Task<ResponseDto<UserResponseDto>> SetRoleToUser(Guid userId, int roleId)
        {
            var data = await _wrapper.RolesRepo.FindItemByCondition(r => r.Id == roleId);
            if (data == null)
                return new ResponseDto<UserResponseDto>(MsgResponce.Role.NotFound(roleId), true);

            var user = await _wrapper.UserRepo.FindByCondition(u => u.Id == userId)
                .Include(i => i.Role)
                .Include(i => i.Permissions)
                .Include(i => i.AccountStatus)
                .FirstOrDefaultAsync();
            if (user == null)
                return new ResponseDto<UserResponseDto>(MsgResponce.User.NotFound, true);

            var permissions = user.Permissions!.Select(x => x).ToList();
            var rolePermissions = data.Permissions.Select(x => x).ToList();
            var removePermissions = permissions.Where(x => rolePermissions.Contains(x.Permission)).ToList();
            if (removePermissions.Any())
            {
                foreach (var item in removePermissions)
                {
                    user.Permissions!.Remove(item);
                }
            }
            user.UserRoleID = roleId;
            user.ModifierId = _userAccessor.UserId;
            await _wrapper.UserRepo.Update(user);
            return new ResponseDto<UserResponseDto>(user.Adapt<UserResponseDto>());
        }
    }
}
