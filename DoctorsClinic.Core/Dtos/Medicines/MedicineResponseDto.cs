using DoctorsClinic.Core.Dtos.PrescriptionMedicines;

namespace DoctorsClinic.Core.Dtos.Medicines
{
    public class MedicineResponseDto
    {
        public required MedicineDto Medicine { get; set; }
        public List<PrescriptionMedicineDto>? PrescriptionMedicines { get; set; }
    }
}