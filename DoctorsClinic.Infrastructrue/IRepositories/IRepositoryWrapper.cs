using DoctorsClinic.Domain.Interfaces;
using DoctorsClinic.Infrastructrue.IRepositories;

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
        IPermissionsRepo PermissionsRepo { get; }
        IRolesRepo RolesRepo { get; }
        Task<bool> SaveAllAsync();
    }
}