using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.MedicalRecords
{
    public class MedicalRecordFilterDto
    {
        public int? PatientID { get; set; }
        public int? DoctorID { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? Diagnosis { get; set; }
    }
}
