using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Core.Dtos.Users;
using Mapster;
using DoctorsClinic.Core.Dtos.Account;
using DoctorsClinic.Core.Dtos.Permission;
using DoctorsClinic.Core.Helper;

namespace DoctorsClinic.Core.Mapster
{
    public class UserMap
    {
        public static void Configure()
        {
            TypeAdapterConfig<CreateUserDto, User>.NewConfig()
                .Ignore(dest => dest.Password)
                .Ignore(dest => dest.Permissions!);

            TypeAdapterConfig<User, UserResponseDto>.NewConfig()
                .Map(dest => dest.AccountStatus, src => src.AccountStatus.Adapt<GetAccountStatus>())
                .Map(dest => dest.Permissions, src => src.Permissions!
                .Select(per => new GetEnum
                {
                    Id = Convert.ToInt32(per.Permission),
                    Name = per.Permission.ToString(),
                    NameAr = per.Permission.GetDescription()
                }).ToList());

            TypeAdapterConfig<User, UserDto>.NewConfig();

            TypeAdapterConfig<User, LoginUserResponse>.NewConfig()
                .Map(dest => dest.Permissions, src => src.Permissions!
                .Select(per => new GetEnum
                {
                    Id = Convert.ToInt32(per.Permission),
                    Name = per.Permission.ToString(),
                    NameAr = per.Permission.GetDescription()
                }).ToList())
                .Ignore(x => x.Token);

            TypeAdapterConfig<UpdateUserDto, User>.NewConfig()
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.FullName), dest => dest.FullName)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.UserName), dest => dest.UserName)
                .IgnoreIf((src, dest) => src.Gender == null, dest => dest.Gender)
                .Ignore(dest => dest.Permissions!)
                .Ignore(dest => dest.Password);

            TypeAdapterConfig<AccountStatus, GetAccountStatus>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.IsActive, src => src.IsActive)
                .Map(dest => dest.IsBlocked, src => src.IsBlocked);
        }
    }
}
