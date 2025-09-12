using DoctorsClinic.Core.Dtos.Doctors;

namespace DoctorsClinic.Core.Dtos.Specialties
{
    public class SpecialtyResponseDto
    {
        public required SpecialtyDto Specialty { get; set; }
        public List<DoctorDto>? Doctors { get; set; }
    }
}