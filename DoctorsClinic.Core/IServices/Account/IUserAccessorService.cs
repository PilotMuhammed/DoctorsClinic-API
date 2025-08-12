using DoctorsClinic.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.IServices.Account
{
    public interface IUserAccessorService : IScopedService
    {
        int UserId { get; }
        string UserName { get; }
        string RoleName { get; }
        string Permissions { get; }
    }
}