using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Core.Dtos.Specialties;
using DoctorsClinic.Core.Dtos.Doctors; 
using Mapster;

namespace DoctorsClinic.Core.Mapster
{
    public class SpecialtyMap
    {
        public static void Configure()
        {
            TypeAdapterConfig<CreateSpecialtyDto, Specialty>.NewConfig()
                .Ignore(dest => dest.Doctors!);

            TypeAdapterConfig<Specialty, SpecialtyResponseDto>.NewConfig()
                .Map(dest => dest.Specialty, src => src.Adapt<SpecialtyDto>()) 
                .Map(dest => dest.Doctors, src => src.Doctors.Adapt<List<DoctorDto>>());

            TypeAdapterConfig<Specialty, SpecialtyDto>.NewConfig();

            TypeAdapterConfig<UpdateSpecialtyDto, Specialty>.NewConfig()
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Name), dest => dest.Name)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Description), dest => dest.Description!)
                .Ignore(dest => dest.Doctors!);
        }
    }
}
