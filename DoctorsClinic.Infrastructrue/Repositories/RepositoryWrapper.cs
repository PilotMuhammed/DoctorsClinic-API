using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly AppDbContext _context;
        public RepositoryWrapper(AppDbContext context)
        {
            _context = context;
        }

        private IAppointmentRepo _appointmentRepo = default!;
        private IDoctorRepo _doctorRepo = default!;
        private IInvoiceRepo _invoiceRepo = default!;
        private IMedicalRecordRepo _medicalRecordRepo = default!;
        private IMedicineRepo _medicineRepo = default!;
        private IPatientRepo _patientRepo = default!;
        private IPaymentRepo _paymentRepo = default!;
        private IPrescriptionMedicineRepo _prescriptionMedicineRepo = default!;
        private IPrescriptionRepo _prescriptionRepo = default!;
        private ISpecialtyRepo _specialtyRepo = default!;
        private IUserRepo _userRepo = default!;

        public IAppointmentRepo AppointmentRepo => _appointmentRepo ??= new AppointmentRepo(_context);
        public IDoctorRepo DoctorRepo => _doctorRepo ??= new DoctorRepo(_context);
        public IInvoiceRepo InvoiceRepo => _invoiceRepo ??= new InvoiceRepo(_context);
        public IMedicalRecordRepo MedicalRecordRepo => _medicalRecordRepo ??= new MedicalRecordRepo(_context);
        public IMedicineRepo MedicineRepo => _medicineRepo ??= new MedicineRepo(_context);
        public IPatientRepo PatientRepo => _patientRepo ??= new PatientRepo(_context);
        public IPaymentRepo PaymentRepo => _paymentRepo ??= new PaymentRepo(_context);
        public IPrescriptionMedicineRepo PrescriptionMedicineRepo => _prescriptionMedicineRepo ??= new PrescriptionMedicineRepo(_context);
        public IPrescriptionRepo PrescriptionRepo => _prescriptionRepo ??= new PrescriptionRepo(_context);
        public ISpecialtyRepo SpecialtyRepo => _specialtyRepo ??= new SpecialtyRepo(_context);
        public IUserRepo UserRepo => _userRepo ??= new UserRepo(_context);

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}