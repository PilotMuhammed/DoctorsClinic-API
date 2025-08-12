using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Account;
using DoctorsClinic.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.IServices.Account
{
    public interface IAuthService : IScopedService
    {
        Task<ResponseDto<LoginUserResponse>> LoginAsync(LoginUser dto, CancellationToken ct = default);
    }
}