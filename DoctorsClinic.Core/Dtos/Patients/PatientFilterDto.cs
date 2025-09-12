using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Dtos.Patients
{
    public class PatientFilterDto
    {
        public string? FullName { get; set; }
        public Gender? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}