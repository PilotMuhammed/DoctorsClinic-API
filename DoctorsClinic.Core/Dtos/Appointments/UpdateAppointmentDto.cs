using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Dtos.Appointments
{
    public class UpdateAppointmentDto
    {
        public int? PatientID { get; set; }
        public int? DoctorID { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public AppointmentStatus? Status { get; set; }
        public string? Notes { get; set; }
    }
}