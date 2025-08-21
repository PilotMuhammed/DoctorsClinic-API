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

            TypeAdapterConfig<Medicine, MedicineDto>.NewConfig();

            TypeAdapterConfig<MedicineDto, Medicine>.NewConfig()
                .Map(d => d.Name, s => s.Name ?? string.Empty)
                .Map(d => d.Description, s => s.Description ?? string.Empty)
                .Map(d => d.Type, s => s.Type ?? string.Empty)
                .Ignore(d => d.PrescriptionMedicines!);

            TypeAdapterConfig<CreateMedicineDto, Medicine>.NewConfig()
                .Map(d => d.Name, s => s.Name) 
                .Map(d => d.Description, s => s.Description ?? string.Empty)
                .Map(d => d.Type, s => s.Type ?? string.Empty)
                .Ignore(d => d.MedicineID)
                .Ignore(d => d.PrescriptionMedicines!);

            TypeAdapterConfig<UpdateMedicineDto, Medicine>.NewConfig()
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Name), d => d.Name)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Description), d => d.Description)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Type), d => d.Type)
                .Ignore(d => d.PrescriptionMedicines!);

            TypeAdapterConfig<Medicine, MedicineResponseDto>.NewConfig()
                .Map(d => d.Medicine, s => s.Adapt<MedicineDto>()!) 
                .Map(d => d.PrescriptionMedicines, _ => (List<PrescriptionMedicineDto>?)null);
        }
    }
}
