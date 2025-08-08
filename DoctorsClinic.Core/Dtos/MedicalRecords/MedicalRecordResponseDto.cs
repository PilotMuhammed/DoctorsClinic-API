using DoctorsClinic.Core.Dtos.Doctors;
using DoctorsClinic.Core.Dtos.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.MedicalRecords
{
    public class MedicalRecordResponseDto
    {
        public required MedicalRecordDto MedicalRecord { get; set; }
        public PatientDto? Patient { get; set; }
        public DoctorDto? Doctor { get; set; }
    }
}
