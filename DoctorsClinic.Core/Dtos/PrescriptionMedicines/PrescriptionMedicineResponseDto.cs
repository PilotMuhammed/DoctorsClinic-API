using DoctorsClinic.Core.Dtos.Medicines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.PrescriptionMedicines
{
    public class PrescriptionMedicineResponseDto
    {
        public required PrescriptionMedicineDto PrescriptionMedicine { get; set; }
        public MedicineDto? Medicine { get; set; }
    }
}
