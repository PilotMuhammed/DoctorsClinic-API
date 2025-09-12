using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Core.Dtos.Appointments
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public int? PatientID { get; set; }
        public string? PatientName { get; set; }      
        public int? DoctorID { get; set; }
        public string? DoctorName { get; set; }       
        public DateTime? AppointmentDate { get; set; }
        public AppointmentStatus? Status { get; set; }            
        public string? Notes { get; set; }
    }
}