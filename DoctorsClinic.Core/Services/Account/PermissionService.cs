using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Permission;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices.Account;
using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Domain.Enums;
using DoctorsClinic.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DoctorsClinic.Core.Services.Account
{
    public class PermissionService : IPermissionService
    {
        private readonly IRepositoryWrapper _wrapper;
        public PermissionService(IRepositoryWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        public async Task<ResponseDto<List<GetEnum>>> Set(Guid id, List<int> pers)
        {
            var user = await _wrapper.UserRepo.GetAll()
                .Include(u => u.Role)
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return new ResponseDto<List<GetEnum>>(MsgResponce.User.NotFound, true);

            var invalidPermissions = pers.Where(p => !Enum.IsDefined(typeof(EPermission), p)).ToList();
            if (invalidPermissions.Any())
                return new ResponseDto<List<GetEnum>>(MsgResponce.Permission.Invalid(invalidPermissions), true);

            var ePermissions = pers.Select(p => (EPermission)p).ToList();
            var existingPermissions = user.Role!.Permissions.ToList();
            var invalidPermissionsForRole = ePermissions.Where(p => !existingPermissions.Contains(p)).ToList();

            user.Permissions!.Clear();

            foreach (var permission in invalidPermissionsForRole)
            {
                if (!user.Permissions.Any(up => up.Permission == permission))
                {
                    user.Permissions.Add(new UserPermission
                    {
                        UserID = user.Id,
                        User = user,
                        Permission = permission
                    });
                }
            }

            var permissions = user.Permissions
                .Select(per => new GetEnum
                {
                    Id = Convert.ToInt32(per.Permission),
                    Name = per.Permission.ToString(),
                    NameAr = per.Permission.GetDescription()
                })
                .ToList();

            if (await _wrapper.SaveAllAsync())
                return new ResponseDto<List<GetEnum>>(permissions);

            return new ResponseDto<List<GetEnum>>(MsgResponce.Failed, true);
        }

        public async Task<ResponseDto<List<GetEnum>>> GetByUserId(Guid id)
        {
            var user = await _wrapper.UserRepo.FindByCondition(u => u.Id == id)
               .Include(i => i.Permissions)
               .FirstOrDefaultAsync();

            if (user == null)
                return new ResponseDto<List<GetEnum>>(MsgResponce.User.NotFound, true);

            var permissions = user.Permissions!
                 .Select(per => new GetEnum
                 {
                     Id = Convert.ToInt32(per.Permission),
                     Name = per.Permission.ToString(),
                     NameAr = per.Permission.GetDescription()
                 })
                 .ToList();

            return new ResponseDto<List<GetEnum>>(permissions);
        }

        public List<GetEnum> GetAll()
        {
            var permissions = Enum.GetValues(typeof(EPermission))
                                  .Cast<EPermission>()
                                  .Select(permission => new GetEnum
                                  {
                                      Id = (int)permission,
                                      Name = permission.ToString(),
                                      NameAr = permission.GetDescription()
                                  })
                                  .ToList();
            return permissions;
        }
    }
}
