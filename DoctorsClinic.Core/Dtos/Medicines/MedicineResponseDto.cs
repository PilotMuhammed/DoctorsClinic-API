using DoctorsClinic.Core.Dtos.PrescriptionMedicines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Medicines
{
    public class MedicineResponseDto
    {
        public required MedicineDto Medicine { get; set; }
        public List<PrescriptionMedicineDto>? PrescriptionMedicines { get; set; }
    }
}
