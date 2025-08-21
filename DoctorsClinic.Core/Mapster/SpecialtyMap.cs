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

            TypeAdapterConfig<Specialty, SpecialtyDto>.NewConfig();

            TypeAdapterConfig<SpecialtyDto, Specialty>.NewConfig()
                .Ignore(d => d.Doctors!);

            TypeAdapterConfig<CreateSpecialtyDto, Specialty>.NewConfig()
                .Ignore(d => d.SpecialtyID)
                .Ignore(d => d.Doctors!);

            TypeAdapterConfig<UpdateSpecialtyDto, Specialty>.NewConfig()
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Name), d => d.Name)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Description), d => d.Description)
                .Ignore(d => d.Doctors!);

            TypeAdapterConfig<Specialty, SpecialtyResponseDto>.NewConfig()
                .Map(d => d.Specialty, s => s.Adapt<SpecialtyDto>()!) 
                .Map(d => d.Doctors, _ => (List<DoctorDto>?)null); 
        }
    }
}
