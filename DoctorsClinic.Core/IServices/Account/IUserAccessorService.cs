using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices.Account
{
    public interface IUserAccessorService : IScopedService
    {
        Guid UserId { get; }
        string UserName { get; }
        string RoleName { get; }
        string Permissions { get; }
    }
}