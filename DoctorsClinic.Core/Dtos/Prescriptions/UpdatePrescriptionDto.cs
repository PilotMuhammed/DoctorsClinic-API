using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Prescriptions
{
    public class UpdatePrescriptionDto
    {
        public int PrescriptionID { get; set; }
        public int? AppointmentID { get; set; }
        public int? DoctorID { get; set; }
        public int? PatientID { get; set; }
        public DateTime? Date { get; set; }
        public string? Notes { get; set; }
    }
}
