using DoctorsClinic.Core.Dtos.Appointments;
using DoctorsClinic.Core.Dtos.Doctors;
using DoctorsClinic.Core.Dtos.Patients;
using DoctorsClinic.Core.Dtos.PrescriptionMedicines;

namespace DoctorsClinic.Core.Dtos.Prescriptions
{
    public class PrescriptionResponseDto
    {
        public PrescriptionDto? Prescription { get; set; }
        public AppointmentDto? Appointment { get; set; }
        public DoctorDto? Doctor { get; set; }
        public PatientDto? Patient { get; set; }
        public List<PrescriptionMedicineDto>? PrescriptionMedicines { get; set; }
    }
}