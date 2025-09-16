using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Core.Dtos.Medicines;
using DoctorsClinic.Core.Dtos.PrescriptionMedicines;
using Mapster;

namespace DoctorsClinic.Core.Mapster
{
    public class MedicineMap
    {
        public static void Configure()
        {
            TypeAdapterConfig<CreateMedicineDto, Medicine>.NewConfig()
                .Ignore(dest => dest.PrescriptionMedicines!);

            TypeAdapterConfig<Medicine, MedicineResponseDto>.NewConfig()
                .Map(dest => dest.Medicine, src => src.Adapt<MedicineDto>()!) 
                .Map(dest => dest.PrescriptionMedicines, src => src.PrescriptionMedicines.Adapt<List<PrescriptionMedicineDto>>());

            TypeAdapterConfig<Medicine, MedicineDto>.NewConfig();

            TypeAdapterConfig<UpdateMedicineDto, Medicine>.NewConfig()
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Name), dest => dest.Name)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Description), dest => dest.Description)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Type), dest => dest.Type)
                .Ignore(dest => dest.PrescriptionMedicines!);
        }
    }
}
