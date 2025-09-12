using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Dtos.Appointments
{
    public class CreateAppointmentDto
    {
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public required AppointmentStatus Status { get; set; }       
        public string? Notes { get; set; }
    }
}