using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Medicines
{
    public class CreateMedicineDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
    }
}
