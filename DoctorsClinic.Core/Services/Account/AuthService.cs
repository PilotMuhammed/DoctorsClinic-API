using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Account;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices.Account;
using DoctorsClinic.Domain.Helper;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DoctorsClinic.Core.Services.Account
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly JWTGenerate _jwtGenerate;
        public AuthService(IRepositoryWrapper wrapper, IConfiguration config)
        {
            _wrapper = wrapper;
            _jwtGenerate = new JWTGenerate(config);
        }

        public async Task<ResponseDto<LoginUserResponse>> Login(LoginUser form)
        {
            var user = await _wrapper.UserRepo.FindByCondition(u => u.UserName.ToLower() == form.UserName.ToLower())
                .Include(u => u.Role)
                .Include(u => u.Permissions)
                .Include(u => u.AccountStatus)
                .FirstOrDefaultAsync();

            if (user == null || user.IsDeleted)
                return new ResponseDto<LoginUserResponse>(MsgResponce.User.NotFound, true);
            if (user.AccountStatus.IsBlocked)
                return new ResponseDto<LoginUserResponse>(MsgResponce.User.banned, true);
            if (!user.AccountStatus.IsActive)
                return new ResponseDto<LoginUserResponse>(MsgResponce.User.NotActive, true);

            var status = user.AccountStatus;

            if (status.IsLocked)
            {
                if (status.LockDateTime.HasValue && status.LockDateTime.Value > DateTime.Now)
                {
                    return new ResponseDto<LoginUserResponse>(MsgResponce.AccountStatus.Locked, true);
                }
                else
                {
                    status.IsLocked = false;
                    status.FailedCount = 0;
                    status.LockDateTime = null;
                    status.Reason = null;
                    await _wrapper.UserRepo.Update(user);
                    await _wrapper.SaveAllAsync();
                }
            }

            if (!string.IsNullOrWhiteSpace(user.Password) && !PasswordHasher.CheckPassword(user.Password, form.Password))
            {
                status.FailedCount++;

                if (status.FailedCount >= 5)
                {
                    status.IsLocked = true;
                    status.LockDateTime = DateTime.Now.AddMinutes(10);
                    status.Reason = MsgResponce.Password.Wrong;
                }

                await _wrapper.UserRepo.Update(user);
                await _wrapper.SaveAllAsync();

                if (status.IsLocked)
                    return new ResponseDto<LoginUserResponse>(MsgResponce.AccountStatus.LockedTenMinutes, true);

                return new ResponseDto<LoginUserResponse>(MsgResponce.Password.Wrong, true);
            }

            if (status.FailedCount > 0)
            {
                status.FailedCount = 0;
                await _wrapper.UserRepo.Update(user);
                await _wrapper.SaveAllAsync();
            }

            var tokenData = new GenerateTokenDto
            {
                Id = user.Id,
                User = user,
                Role = user.Role!,
                Permissions = user.Permissions!.ToList()
            };

            var userLogin = user.Adapt<LoginUserResponse>();
            userLogin.Token = _jwtGenerate.GenerateJwtToken(tokenData);

            return new ResponseDto<LoginUserResponse>(userLogin);
        }
    }
}
