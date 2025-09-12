using DoctorsClinic.Core.Dtos.Medicines;

namespace DoctorsClinic.Core.Dtos.PrescriptionMedicines
{
    public class PrescriptionMedicineResponseDto
    {
        public required PrescriptionMedicineDto PrescriptionMedicine { get; set; }
        public MedicineDto? Medicine { get; set; }
    }
}