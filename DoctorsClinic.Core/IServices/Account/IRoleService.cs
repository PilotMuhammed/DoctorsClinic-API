using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Role;
using DoctorsClinic.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.IServices.Account
{
    public interface IRoleService : IScopedService
    {
        ResponseDto<List<GetRole>> GetAll();
        ResponseDto<List<ListDto<int>>> GetList();
    }
}
