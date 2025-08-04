using System;
using System.Collections.Generic;
using DoctorsClinic.Domain.Enums;

namespace DoctorsClinic.Domain.Entities
{
    public class Patient
    {
        public int PatientID { get; set; }
        public string? FullName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DOB { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
