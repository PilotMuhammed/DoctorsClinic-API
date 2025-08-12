using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Domain.Enums;
using DoctorsClinic.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.IServices.Account
{
    public interface IPermissionService : IScopedService
    {
        ResponseDto<List<string>> GetAll();

        ResponseDto<List<string>> GetByRole(UserRole role);
    }
}
