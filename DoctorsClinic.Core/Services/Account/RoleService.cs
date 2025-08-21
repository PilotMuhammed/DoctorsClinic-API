using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Role;
using DoctorsClinic.Core.IServices.Account;
using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Services.Account
{
    public class RoleService : IRoleService
    {
        public ResponseDto<List<GetRole>> GetAll()
        {
            var roles = Enum.GetValues(typeof(UserRole))
                .Cast<UserRole>()
                .Select(r => new GetRole
                {
                    Id = (int)r,
                    Name = r.ToString()
                })
                .ToList();

            return new ResponseDto<List<GetRole>>(roles);
        }

        public ResponseDto<List<ListDto<int>>> GetList()
        {
            var ids = Enum.GetValues(typeof(UserRole))
                .Cast<UserRole>()
                .Select(r => (int)r)
                .ToList();

            var listDto = new ListDto<int>
            {
                Items = ids,
                TotalCount = ids.Count
            };
            return new ResponseDto<List<ListDto<int>>>(new List<ListDto<int>> { listDto });
        }
    }
}
