using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.MedicalRecords
{
    public class MedicalRecordDto
    {
        public int RecordID { get; set; }
        public int PatientID { get; set; }
        public string? PatientName { get; set; }    
        public int DoctorID { get; set; }
        public string? DoctorName { get; set; }     
        public required string Diagnosis { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
    }
}
