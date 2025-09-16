using DoctorsClinic.Core.Dtos.Permission;
using DoctorsClinic.Core.Dtos.Role;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Domain.Enums;
using Mapster;

namespace DoctorsClinic.Core.Mapster
{
    public class RoleMap
    {
        public static void Configure()
        {
            TypeAdapterConfig<CreateRole, UserRole>.NewConfig()
                .Ignore(dest => dest.Permissions);

            TypeAdapterConfig<UserRole, GetRole>.NewConfig()
                .Map(dest => dest.Permissions, src => src.Permissions.Select(per => new GetEnum
                {
                    Id = Convert.ToInt32(per),
                    Name = ((EPermission)per).ToString(),
                    NameAr = ((EPermission)per).GetDescription()
                }).ToList());

            TypeAdapterConfig<UserRole, RoleDto>.NewConfig();

            TypeAdapterConfig<UpdateRole, UserRole>.NewConfig()
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Name), dest => dest.Name)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.NameAr), dest => dest.NameAr)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Description), dest => dest.Description!)
                .Ignore(dest => dest.Permissions);
        }
    }
}
