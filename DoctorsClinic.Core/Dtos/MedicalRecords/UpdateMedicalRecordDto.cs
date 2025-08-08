using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.MedicalRecords
{
    public class UpdateMedicalRecordDto
    {
        public int RecordID { get; set; }
        public int? PatientID { get; set; }
        public int? DoctorID { get; set; }
        public string? Diagnosis { get; set; }
        public DateTime? Date { get; set; }
        public string? Notes { get; set; }
    }
}
