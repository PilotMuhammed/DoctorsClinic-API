using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Domain.Entities
{
    public class Patient : BaseEntity<int>
    {
        public required string FullName { get; set; }
        public Gender Gender { get; set; } = Gender.undefined;
        public DateOnly DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<MedicalRecord>? MedicalRecords { get; set; }
        public ICollection<Prescription>? Prescriptions { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }
    }
}
