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

            TypeAdapterConfig<PrescriptionMedicine, PrescriptionMedicineDto>.NewConfig()
                .Map(d => d.MedicineName, s => s.Medicine != null ? s.Medicine.Name : null);

            TypeAdapterConfig<PrescriptionMedicineDto, PrescriptionMedicine>.NewConfig()
                .Map(d => d.Dose, s => s.Dose ?? string.Empty)
                .Map(d => d.Duration, s => s.Duration ?? string.Empty)
                .Map(d => d.Instructions, s => s.Instructions ?? string.Empty)
                .Ignore(d => d.Prescription)
                .Ignore(d => d.Medicine);

            TypeAdapterConfig<CreatePrescriptionMedicineDto, PrescriptionMedicine>.NewConfig()
                .Map(d => d.Dose, s => s.Dose ?? string.Empty)
                .Map(d => d.Duration, s => s.Duration ?? string.Empty)
                .Map(d => d.Instructions, s => s.Instructions ?? string.Empty)
                .Ignore(d => d.ID)
                .Ignore(d => d.Prescription)
                .Ignore(d => d.Medicine);

            TypeAdapterConfig<UpdatePrescriptionMedicineDto, PrescriptionMedicine>.NewConfig()
                .IgnoreIf((s, _) => s.PrescriptionID == null, d => d.PrescriptionID)
                .IgnoreIf((s, _) => s.MedicineID == null, d => d.MedicineID)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Dose), d => d.Dose)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Duration), d => d.Duration)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.Instructions), d => d.Instructions)
                .Ignore(d => d.Prescription)
                .Ignore(d => d.Medicine);

            TypeAdapterConfig<PrescriptionMedicine, PrescriptionMedicineResponseDto>.NewConfig()
                .Map(d => d.PrescriptionMedicine, s => s.Adapt<PrescriptionMedicineDto>()!) 
                .Map(d => d.Medicine, _ => (MedicineDto?)null); 
        }
    }
}
