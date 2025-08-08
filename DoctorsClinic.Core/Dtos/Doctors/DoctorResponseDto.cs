using DoctorsClinic.Core.Dtos.Appointments;
using DoctorsClinic.Core.Dtos.MedicalRecords;
using DoctorsClinic.Core.Dtos.Prescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Doctors
{
    public class DoctorResponseDto
    {
        public required DoctorDto Doctor { get; set; }
        public List<AppointmentDto>? Appointments { get; set; }
        public List<MedicalRecordDto>? MedicalRecords { get; set; }
        public List<PrescriptionDto>? Prescriptions { get; set; }
    }
}
