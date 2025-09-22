using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Core.Dtos.PrescriptionMedicines;
using DoctorsClinic.Core.Dtos.Medicines; 
using Mapster;

namespace DoctorsClinic.Core.Mapster
{
    public class PrescriptionMedicineMap
    {
        public static void Configure()
        {
            TypeAdapterConfig<CreatePrescriptionMedicineDto, PrescriptionMedicine>.NewConfig();

            TypeAdapterConfig<PrescriptionMedicine, PrescriptionMedicineResponseDto>.NewConfig()
                .Map(dest => dest.PrescriptionMedicine, src => src.Adapt<PrescriptionMedicineDto>())
                .Map(dest => dest.Medicine, src => src.Medicine.Adapt<MedicineDto>());

            TypeAdapterConfig<PrescriptionMedicine, PrescriptionMedicineDto>.NewConfig()
                .Map(dest => dest.MedicineName, src => src.Medicine!.Name);

            TypeAdapterConfig<UpdatePrescriptionMedicineDto, PrescriptionMedicine>.NewConfig()
                .IgnoreIf((src, dest) => src.PrescriptionID == null, dest => dest.PrescriptionID)
                .IgnoreIf((src, dest) => src.MedicineID == null, dest => dest.MedicineID)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Dose), dest => dest.Dose)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Duration), dest => dest.Duration)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Instructions), dest => dest.Instructions);
        }
    }
}
