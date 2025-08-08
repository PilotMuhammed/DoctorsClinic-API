using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Appointments
{
    public class AppointmentDto
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public string? PatientName { get; set; }      
        public int DoctorID { get; set; }
        public string? DoctorName { get; set; }       
        public DateTime AppointmentDate { get; set; }
        public required string Status { get; set; }            
        public string? Notes { get; set; }
    }
}
