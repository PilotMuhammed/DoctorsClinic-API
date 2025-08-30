using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorsClinic.Infrastructure.IRepositories;

namespace DoctorsClinic.Infrastructure.IRepositories
{
    public interface IRepositoryWrapper
    {
        IPatientRepo Patients { get; }
        IDoctorRepo Doctors { get; }
        IAppointmentRepo Appointments { get; }
        ISpecialtyRepo Specialties { get; }
        IMedicalRecordRepo MedicalRecords { get; }
        IPrescriptionRepo Prescriptions { get; }
        IMedicineRepo Medicines { get; }
        IInvoiceRepo Invoices { get; }
        IPaymentRepo Payments { get; }
        IUserRepo Users { get; }
        IPrescriptionMedicineRepo PrescriptionMedicines { get; }
        Task<int> SaveChangesAsync();
    }
}

