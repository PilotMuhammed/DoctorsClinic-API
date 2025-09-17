using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Account;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices.Account
{
    public interface IAuthService : IScopedService
    {
        Task<ResponseDto<LoginUserResponse>> Login(LoginUser form);
    }
}