using DoctorsClinic.Core.Dtos.Doctors;
using DoctorsClinic.Core.Dtos.Patients;

namespace DoctorsClinic.Core.Dtos.MedicalRecords
{
    public class MedicalRecordResponseDto
    {
        public MedicalRecordDto? MedicalRecord { get; set; }
        public PatientDto? Patient { get; set; }
        public DoctorDto? Doctor { get; set; }
    }
}