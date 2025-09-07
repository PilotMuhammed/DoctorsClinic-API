using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DoctorsClinic.Domain.Helper;

namespace DoctorsClinic.Infrastructure.Data
{
    public class DbSeed
    {
        public static async Task SeedAsync(AppDbContext db, ILogger<DbSeed> logger)
        {
            if (!await db.Specialties.AnyAsync())
            {
                var specialties = GetPreconfiguredSpecialties();
                db.Specialties.AddRange(specialties);
                await db.SaveChangesAsync();
            }
            if (!await db.Medicines.AnyAsync())
            {
                var medicines = GetPreconfiguredMedicines();
                db.Medicines.AddRange(medicines);
                await db.SaveChangesAsync();
            }
            if (!await db.Users.AnyAsync())
            {
                var users = GetPreconfiguredUsers();
                db.Users.AddRange(users);
                await db.SaveChangesAsync();
            }
            if (!await db.Doctors.AnyAsync())
            {
                var doctors = GetPreconfiguredDoctors(db);
                db.Doctors.AddRange(doctors);
                await db.SaveChangesAsync();
            }
            if (!await db.Patients.AnyAsync())
            {
                var patients = GetPreconfiguredPatients();
                db.Patients.AddRange(patients);
                await db.SaveChangesAsync();
            }
            if (!await db.Appointments.AnyAsync())
            {
                var appointments = GetPreconfiguredAppointments(db);
                db.Appointments.AddRange(appointments);
                await db.SaveChangesAsync();
            }
            if (!await db.Prescriptions.AnyAsync())
            {
                var prescriptions = GetPreconfiguredPrescriptions(db);
                db.Prescriptions.AddRange(prescriptions);
                await db.SaveChangesAsync();
            }
            if (!await db.MedicalRecords.AnyAsync())
            {
                var records = GetPreconfiguredMedicalRecords(db);
                db.MedicalRecords.AddRange(records);
                await db.SaveChangesAsync();
            }
            if (!await db.Invoices.AnyAsync())
            {
                var invoices = GetPreconfiguredInvoices(db);
                db.Invoices.AddRange(invoices);
                await db.SaveChangesAsync();
            }
            if (!await db.Payments.AnyAsync())
            {
                var payments = GetPreconfiguredPayments(db);
                db.Payments.AddRange(payments);
                await db.SaveChangesAsync();
            }

            logger.LogInformation("Seeding completed successfully!");
        }

        private static List<Specialty> GetPreconfiguredSpecialties() => new()
        {
            new Specialty { Id = 1, Name = "Cardiology", Description = "Heart specialist" },
            new Specialty { Id = 2, Name = "ENT", Description = "Ear, Nose, and Throat specialist" },
            new Specialty { Id = 3, Name = "Orthopedics", Description = "Bones and joints" },
            new Specialty { Id = 4, Name = "Dermatology", Description = "Skin specialist" }
        };

        private static List<Medicine> GetPreconfiguredMedicines() => new()
        {
            new Medicine { Id = 1, Name = "Aspirin", Description = "Pain reliever", Type = "Tablet" },
            new Medicine { Id = 2, Name = "Amoxicillin", Description = "Antibiotic", Type = "Capsule" },
            new Medicine { Id = 3, Name = "Lisinopril", Description = "Blood pressure", Type = "Tablet" }
        };

        private static List<User> GetPreconfiguredUsers() => new()
        {
            new User
            {
                Id = 1,
                Username = "ahmed@admin",
                PasswordHash = PasswordHasher.HashPassword("12345"), 
                Role = UserRole.Admin 
            },
            new User
            {
                Id = 2,
                Username = "abaas@doctor",
                PasswordHash = PasswordHasher.HashPassword("12345"),
                Role = UserRole.Doctor 
            },
            new User
            {
                Id = 3,
                Username = "hussain@reception",
                PasswordHash = PasswordHasher.HashPassword("12345"),
                Role = UserRole.Receptionist 
            },
            new User
            {
                Id = 4,
                Username = "mariam@nurse",
                PasswordHash = PasswordHasher.HashPassword("12345"),
                Role = UserRole.Nurse 
            }
        };

        private static List<Doctor> GetPreconfiguredDoctors(AppDbContext db)
        {
            var specialties = db.Specialties.ToList();
            var users = db.Users.ToList();

            return new List<Doctor>
            {
                new Doctor
                {
                    Id = 1,
                    FullName = "Dr. Abbas al-Numani",
                    SpecialtyID = specialties.FirstOrDefault(s => s.Name == "Cardiology")?.Id ?? 1,
                    Phone = "07712345678",
                    Email = "abbas@gmail.com",
                    UserID = users.FirstOrDefault(u => u.Username == "abaas@doctor")?.Id ?? 2
                },
            };
        }

        private static List<Patient> GetPreconfiguredPatients() => new()
        {
            new Patient
            {
                Id = 1,
                FullName = "Sabah Ali",
                Gender = Gender.Male,
                DateOfBirth = new DateOnly(1995, 1, 15),
                Phone = "07801234567",
                Address = "Karbala"
            },
            new Patient
            {
                Id = 2,
                FullName = "Sara Mohammed",
                Gender = Gender.Female,
                DateOfBirth = new DateOnly(2001, 8, 27),
                Phone = "07801234568",
                Address = "Baghdad"
            }
        };

        private static List<Appointment> GetPreconfiguredAppointments(AppDbContext db)
        {
            var doctor = db.Doctors.FirstOrDefault();
            var patient = db.Patients.FirstOrDefault();

            return new List<Appointment>
            {
                new Appointment
                {
                    Id = 1,
                    PatientID = patient?.Id ?? 1,
                    DoctorID = doctor?.Id ?? 1,
                    AppointmentDate = DateTime.UtcNow.AddDays(-2),
                    Status = AppointmentStatus.Completed,
                    Notes = "Routine check-up"
                },
                new Appointment
                {
                    Id = 2,
                    PatientID = patient?.Id ?? 2,
                    DoctorID = doctor?.Id ?? 1,
                    AppointmentDate = DateTime.UtcNow.AddDays(3),
                    Status = AppointmentStatus.Scheduled,
                    Notes = "Consultation"
                }
            };
        }

        private static List<Prescription> GetPreconfiguredPrescriptions(AppDbContext db)
        {
            var appointment = db.Appointments.FirstOrDefault();
            var doctor = db.Doctors.FirstOrDefault();
            var patient = db.Patients.FirstOrDefault();

            var medicines = db.Medicines.ToList();

            return new List<Prescription>
            {
                new Prescription
                {
                    Id = 1,
                    AppointmentID = appointment?.Id ?? 1,
                    DoctorID = doctor?.Id ?? 1,
                    PatientID = patient?.Id ?? 1,
                    Date = DateTime.UtcNow.AddDays(-2),
                    Notes = "Take two tablets daily.",
                    PrescriptionMedicines = new List<PrescriptionMedicine>
                    {
                        new PrescriptionMedicine
                        {
                            Id = 1,
                            MedicineID = medicines.FirstOrDefault(m => m.Name == "Aspirin")?.Id ?? 1,
                            Dose = "100mg",
                            Duration = "7 days",
                            Instructions = "After meal"
                        }
                    }
                }
            };
        }

        private static List<MedicalRecord> GetPreconfiguredMedicalRecords(AppDbContext db)
        {
            var doctor = db.Doctors.FirstOrDefault();
            var patient = db.Patients.FirstOrDefault();

            return new List<MedicalRecord>
            {
                new MedicalRecord
                {
                    Id = 1,
                    PatientID = patient?.Id ?? 1,
                    DoctorID = doctor?.Id ?? 1,
                    Diagnosis = "High blood pressure",
                    Date = DateTime.UtcNow.AddDays(-2),
                    Notes = "Monitor blood pressure daily"
                }
            };
        }

        private static List<Invoice> GetPreconfiguredInvoices(AppDbContext db)
        {
            var patient = db.Patients.FirstOrDefault();
            var appointment = db.Appointments.FirstOrDefault();

            return new List<Invoice>
            {
                new Invoice
                {
                    Id = 1,
                    PatientID = patient?.Id ?? 1,
                    AppointmentID = appointment?.Id ?? 1,
                    TotalAmount = 35000,
                    Status = InvoiceStatus.Paid,
                    Date = DateTime.UtcNow.AddDays(-1),
                    Payments = new List<Payment>()
                }
            };
        }

        private static List<Payment> GetPreconfiguredPayments(AppDbContext db)
        {
            var invoice = db.Invoices.FirstOrDefault();

            return new List<Payment>
            {
                new Payment
                {
                    Id = 1,
                    InvoiceID = invoice?.Id ?? 1,
                    Amount = 35000,
                    Date = DateTime.UtcNow,
                    PaymentMethod = PaymentMethod.Cash
                }
            };
        }
    }
}