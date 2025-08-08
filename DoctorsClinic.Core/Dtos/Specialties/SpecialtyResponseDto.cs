using DoctorsClinic.Core.Dtos.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Specialties
{
    public class SpecialtyResponseDto
    {
        public required SpecialtyDto Specialty { get; set; }
        public List<DoctorDto>? Doctors { get; set; }
    }
}
