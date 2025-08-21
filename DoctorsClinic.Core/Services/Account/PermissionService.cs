using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Permission;
using DoctorsClinic.Core.Dtos.Role;
using DoctorsClinic.Core.IServices.Account;
using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Services.Account
{
    public class PermissionService : IPermissionService
    {
        public ResponseDto<List<string>> GetAll()
        {
            var permissions = Permissions.All.ToList();
            return new ResponseDto<List<string>>(permissions);
        }

        public ResponseDto<List<string>> GetByRole(UserRole role)
        {
            var rolePermissions = RolePermissionMap.GetPermissions(role).ToList();
            return new ResponseDto<List<string>>(rolePermissions);
        }
    }
}
