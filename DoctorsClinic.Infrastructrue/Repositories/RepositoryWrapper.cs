using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly AppDbContext _context;

        public IPatientRepo Patients { get; }
        public IDoctorRepo Doctors { get; }
        public IAppointmentRepo Appointments { get; }
        public ISpecialtyRepo Specialties { get; }
        public IMedicalRecordRepo MedicalRecords { get; }
        public IPrescriptionRepo Prescriptions { get; }
        public IMedicineRepo Medicines { get; }
        public IInvoiceRepo Invoices { get; }
        public IPaymentRepo Payments { get; }
        public IUserRepo Users { get; }
        public IPrescriptionMedicineRepo PrescriptionMedicines { get; }

        public RepositoryWrapper(AppDbContext context)
        {
            _context = context;
            Patients = new PatientRepo(context);
            Doctors = new DoctorRepo(context);
            Appointments = new AppointmentRepo(context);
            Specialties = new SpecialtyRepo(context);
            MedicalRecords = new MedicalRecordRepo(context);
            Prescriptions = new PrescriptionRepo(context);
            Medicines = new MedicineRepo(context);
            Invoices = new InvoiceRepo(context);
            Payments = new PaymentRepo(context);
            Users = new UserRepo(context);
            PrescriptionMedicines = new PrescriptionMedicineRepo(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
