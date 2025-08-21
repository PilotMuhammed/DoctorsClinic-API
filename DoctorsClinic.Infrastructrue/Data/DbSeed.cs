using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto.Generators;
using DoctorsClinic.Domain.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            new Specialty { SpecialtyID = 1, Name = "Cardiology", Description = "Heart specialist" },
            new Specialty { SpecialtyID = 2, Name = "ENT", Description = "Ear, Nose, and Throat specialist" },
            new Specialty { SpecialtyID = 3, Name = "Orthopedics", Description = "Bones and joints" },
            new Specialty { SpecialtyID = 4, Name = "Dermatology", Description = "Skin specialist" }
        };

        private static List<Medicine> GetPreconfiguredMedicines() => new()
        {
            new Medicine { MedicineID = 1, Name = "Aspirin", Description = "Pain reliever", Type = "Tablet" },
            new Medicine { MedicineID = 2, Name = "Amoxicillin", Description = "Antibiotic", Type = "Capsule" },
            new Medicine { MedicineID = 3, Name = "Lisinopril", Description = "Blood pressure", Type = "Tablet" }
        };

        private static List<User> GetPreconfiguredUsers() => new()
        {
            new User
            {
                UserID = 1,
                Username = "ahmed@admin",
                PasswordHash = PasswordHasher.HashPassword("12345"), 
                Role = UserRole.Admin // 1 => Permission Admin
            },
            new User
            {
                UserID = 2,
                Username = "abaas@doctor",
                PasswordHash = PasswordHasher.HashPassword("12345"),
                Role = UserRole.Doctor // 2 => Permission Doctor
            },
            new User
            {
                UserID = 3,
                Username = "hussain@reception",
                PasswordHash = PasswordHasher.HashPassword("12345"),
                Role = UserRole.Receptionist // 3 => Permission Receptionist
            },
            new User
            {
                UserID = 4,
                Username = "mariam@nurse",
                PasswordHash = PasswordHasher.HashPassword("12345"),
                Role = UserRole.Nurse // 4 => Permission Nurse
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
                    DoctorID = 1,
                    FullName = "Dr. Abbas al-Numani",
                    SpecialtyID = specialties.FirstOrDefault(s => s.Name == "Cardiology")?.SpecialtyID ?? 1,
                    Phone = "07712345678",
                    Email = "abbas@gmail.com",
                    UserID = users.FirstOrDefault(u => u.Username == "abaas@doctor")?.UserID ?? 2
                },
                new Doctor
                {
                    DoctorID = 2,
                    FullName = "Dr. Nasser Shamkhi",
                    SpecialtyID = specialties.FirstOrDefault(s => s.Name == "ENT")?.SpecialtyID ?? 2,
                    Phone = "07712345679",
                    Email = "nasser@gmail.com",
                    UserID = 0 
                }
            };
        }

        private static List<Patient> GetPreconfiguredPatients() => new()
        {
            new Patient
            {
                PatientID = 1,
                FullName = "Sabah Ali",
                Gender = Gender.Male,
                DOB = new DateTime(1995, 1, 15).ToUniversalTime(),
                Phone = "07801234567",
                Email = "sabah@gmail.com",
                Address = "Karbala"
            },
            new Patient
            {
                PatientID = 2,
                FullName = "Sara Mohammed",
                Gender = Gender.Female,
                DOB = new DateTime(2001, 8, 27).ToUniversalTime(),
                Phone = "07801234568",
                Email = "sara@gmail.com",
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
                    AppointmentID = 1,
                    PatientID = patient?.PatientID ?? 1,
                    DoctorID = doctor?.DoctorID ?? 1,
                    AppointmentDate = DateTime.UtcNow.AddDays(-2),
                    Status = AppointmentStatus.Completed,
                    Notes = "Routine check-up"
                },
                new Appointment
                {
                    AppointmentID = 2,
                    PatientID = patient?.PatientID ?? 2,
                    DoctorID = doctor?.DoctorID ?? 1,
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
                    PrescriptionID = 1,
                    AppointmentID = appointment?.AppointmentID ?? 1,
                    DoctorID = doctor?.DoctorID ?? 1,
                    PatientID = patient?.PatientID ?? 1,
                    Date = DateTime.UtcNow.AddDays(-2),
                    Notes = "Take two tablets daily.",
                    PrescriptionMedicines = new List<PrescriptionMedicine>
                    {
                        new PrescriptionMedicine
                        {
                            ID = 1,
                            MedicineID = medicines.FirstOrDefault(m => m.Name == "Aspirin")?.MedicineID ?? 1,
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
                    RecordID = 1,
                    PatientID = patient?.PatientID ?? 1,
                    DoctorID = doctor?.DoctorID ?? 1,
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
                    InvoiceID = 1,
                    PatientID = patient?.PatientID ?? 1,
                    AppointmentID = appointment?.AppointmentID ?? 1,
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
                    PaymentID = 1,
                    InvoiceID = invoice?.InvoiceID ?? 1,
                    Amount = 35000,
                    Date = DateTime.UtcNow,
                    PaymentMethod = PaymentMethod.Cash
                }
            };
        }
    }
}
