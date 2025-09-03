using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Infrastructure.IRepositories
{
    public interface IRepositoryWrapper : IScopedService
    {
        IAppointmentRepo AppointmentRepo { get; }
        IDoctorRepo DoctorRepo { get; }
        IInvoiceRepo InvoiceRepo { get; }
        IMedicalRecordRepo MedicalRecordRepo { get; }
        IMedicineRepo MedicineRepo { get; }
        IPatientRepo PatientRepo { get; }
        IPaymentRepo PaymentRepo { get; }
        IPrescriptionMedicineRepo PrescriptionMedicineRepo { get; }
        IPrescriptionRepo PrescriptionRepo { get; }
        ISpecialtyRepo SpecialtyRepo { get; }
        IUserRepo UserRepo { get; }
        Task<bool> SaveAllAsync();
    }
}

