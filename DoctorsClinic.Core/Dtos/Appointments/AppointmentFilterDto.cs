using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Dtos.Appointments
{
    public class AppointmentFilterDto
    {
        public int? PatientID { get; set; }
        public int? DoctorID { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public AppointmentStatus? Status { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}