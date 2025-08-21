using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Account;
using DoctorsClinic.Core.Dtos.Permission;
using DoctorsClinic.Core.Dtos.Role;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices.Account;
using DoctorsClinic.Domain.Helper;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DoctorsClinic.Core.Services.Account
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryWrapper _repo;
        private readonly JWTGenerate _jwt;

        public AuthService(IRepositoryWrapper repo, JWTGenerate jwt)
        {
            _repo = repo;
            _jwt = jwt;
        }

        public async Task<ResponseDto<LoginUserResponse>> LoginAsync(LoginUser dto, CancellationToken ct = default)
        {
            if (dto == null)
                return new ResponseDto<LoginUserResponse>(MsgResponce.Failed, true);

            if (string.IsNullOrWhiteSpace(dto.Username))
                return new ResponseDto<LoginUserResponse>(MsgResponce.User.NotFound, true);

            if (string.IsNullOrWhiteSpace(dto.Password))
                return new ResponseDto<LoginUserResponse>(MsgResponce.Password.Wrong, true);

            var user = await _repo.Users
                .FindByCondition(u => u.Username == dto.Username, track: false)
                .Include(u => u.Doctor)
                .FirstOrDefaultAsync(ct);

            if (user == null)
                return new ResponseDto<LoginUserResponse>(MsgResponce.User.NotFound, true);

            if (!PasswordHasher.CheckPassword(user.PasswordHash, dto.Password))
                return new ResponseDto<LoginUserResponse>(MsgResponce.Password.Wrong, true);

            var rolePermissions = RolePermissionMap.GetPermissions(user.Role);
            var userPermissions = new List<string>();

            var tokenDto = new GenerateTokenDto
            {
                UserId = user.UserID,
                UserName = user.Username,
                RoleName = user.Role.ToString(),
                RolePermissions = rolePermissions,
                UserPermissions = userPermissions
            };

            var token = _jwt.GenerateJwtToken(tokenDto);

            var response = new LoginUserResponse
            {
                UserID = user.UserID,
                Username = user.Username,
                Role = new GetRole { Id = (int)user.Role, Name = user.Role.ToString() },
                DoctorID = user.DoctorID,
                Token = token,
                Permissions = (rolePermissions.Concat(userPermissions)).Distinct()
                    .Select(p => new GetEnum { Name = p }).ToList()
            };

            return new ResponseDto<LoginUserResponse>(response);
        }
    }
}
