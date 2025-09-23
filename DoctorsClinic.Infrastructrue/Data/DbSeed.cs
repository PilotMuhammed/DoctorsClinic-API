using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DoctorsClinic.Domain.Helper;

namespace DoctorsClinic.Infrastructure.Data
{
    public class DbSeed
    {
        public static async Task SeedAsync(AppDbContext appContext, ILogger<DbSeed> logger)
        {
            if (!await appContext.UserRoles.AnyAsync())
            {
                var userRoles = GetPreconfiguredUsersRole();
                appContext.UserRoles.AddRange(userRoles);
                await appContext.SaveChangesAsync();
            }
            if (!await appContext.Users.AnyAsync())
            {
                var users = GetPreconfiguredUsers();
                appContext.Users.AddRange(users);
                await appContext.SaveChangesAsync();
            }
            if (!await appContext.Specialties.AnyAsync())
            {
                var specialties = GetPreconfiguredSpecialties();
                appContext.Specialties.AddRange(specialties);
                await appContext.SaveChangesAsync();
            }
            if (!await appContext.Medicines.AnyAsync())
            {
                var medicines = GetPreconfiguredMedicines();
                appContext.Medicines.AddRange(medicines);
                await appContext.SaveChangesAsync();
            }
            if (!await appContext.Doctors.AnyAsync())
            {
                var doctors = GetPreconfiguredDoctors(appContext);
                appContext.Doctors.AddRange(doctors);
                await appContext.SaveChangesAsync();
            }
            if (!await appContext.Patients.AnyAsync())
            {
                var patients = GetPreconfiguredPatients();
                appContext.Patients.AddRange(patients);
                await appContext.SaveChangesAsync();
            }
            if (!await appContext.Appointments.AnyAsync())
            {
                var appointments = GetPreconfiguredAppointments(appContext);
                appContext.Appointments.AddRange(appointments);
                await appContext.SaveChangesAsync();
            }
            if (!await appContext.Prescriptions.AnyAsync())
            {
                var prescriptions = GetPreconfiguredPrescriptions(appContext);
                appContext.Prescriptions.AddRange(prescriptions);
                await appContext.SaveChangesAsync();
            }
            if (!await appContext.MedicalRecords.AnyAsync())
            {
                var records = GetPreconfiguredMedicalRecords(appContext);
                appContext.MedicalRecords.AddRange(records);
                await appContext.SaveChangesAsync();
            }
            if (!await appContext.Invoices.AnyAsync())
            {
                var invoices = GetPreconfiguredInvoices(appContext);
                appContext.Invoices.AddRange(invoices);
                await appContext.SaveChangesAsync();
            }
            if (!await appContext.Payments.AnyAsync())
            {
                var payments = GetPreconfiguredPayments(appContext);
                appContext.Payments.AddRange(payments);
                await appContext.SaveChangesAsync();
            }

            logger.LogInformation("Seeding completed successfully!");
        }

        private static List<User> GetPreconfiguredUsers()
        {
            var superAdminGuid = Guid.NewGuid();
            var abaasGuid = Guid.NewGuid();
            var hussainGuid = Guid.NewGuid();
            var mariamGuid = Guid.NewGuid();

            return new List<User>
            {
                new User
                {
                    Id = superAdminGuid,
                    FullName = "Super Admin",
                    UserName = "SuperAdmin",
                    Password = PasswordHasher.HashPassword("12345"),
                    UserRoleID = 1,
                    AccountStatus = new AccountStatus { UserId = superAdminGuid, },
                },
                new User
                {
                    Id = abaasGuid,
                    FullName = "Dr. Abaas",
                    UserName = "abaas",
                    Password = PasswordHasher.HashPassword("12345"),
                    UserRoleID = 3,
                    AccountStatus = new AccountStatus { UserId = abaasGuid, },
                },
                new User
                {
                    Id = hussainGuid,
                    FullName = "Reception Hussain",
                    UserName = "hussain",
                    Password = PasswordHasher.HashPassword("12345"),
                    UserRoleID = 4,
                    AccountStatus = new AccountStatus { UserId = hussainGuid, },
                },
                new User
                {
                    Id = mariamGuid,
                    FullName = "Nurse Mariam",
                    UserName = "hussain",
                    Password = PasswordHasher.HashPassword("12345"),
                    UserRoleID = 5,
                    AccountStatus = new AccountStatus { UserId = mariamGuid, },
                }
            };
        }

        private static List<UserRole> GetPreconfiguredUsersRole()
        {
            return new List<UserRole>()
            {
                new UserRole()
                {
                    Id = 1,
                    Name = "Super Admin",
                    NameAr = "مدير النظام",
                    Description = "Super Admin",
                    Permissions = Enum.GetValues<EPermission>().ToList()
                },
                new UserRole()
                {
                    Id = 2,
                    Name = "User",
                    NameAr = "مستخدم عادي",
                    Description = "User",
                    Permissions = []
                },
                new UserRole()
                {
                    Id = 3,
                    Name = "Doctor",
                    NameAr = "الدكتور",
                    Description = "Doctor",
                    Permissions = new List<EPermission>
                    {
                        EPermission.Patients_View,
                        EPermission.MedicalRecords_View,
                        EPermission.MedicalRecords_Create,
                        EPermission.MedicalRecords_Update,
                        EPermission.Prescriptions_View,
                        EPermission.Prescriptions_Create,
                        EPermission.Prescriptions_Update,
                        EPermission.Appointments_View,
                        EPermission.Medicines_View,
                    }
                },
                new UserRole()
                {
                    Id = 4,
                    Name = "Reception",
                    NameAr = "موظف الاستقبال",
                    Description = "Reception",
                    Permissions = new List<EPermission>
                    {
                        EPermission.Patients_View,
                        EPermission.Patients_Create,
                        EPermission.Patients_Update,
                        EPermission.Appointments_View,
                        EPermission.Appointments_Create,
                        EPermission.Appointments_Update,
                        EPermission.Appointments_Delete,
                        EPermission.Doctors_View,
                        EPermission.Specialties_View,
                        EPermission.Invoices_View,
                        EPermission.Invoices_Create,
                        EPermission.Invoices_Update,
                        EPermission.Payments_View,
                        EPermission.Payments_Create,
                        EPermission.Payments_Update,
                    }
                },
                new UserRole()
                {
                    Id = 5,
                    Name = "Nurse",
                    NameAr = "الممرضة",
                    Description = "Nurse",
                    Permissions = new List<EPermission>
                    {
                        EPermission.Patients_View,
                        EPermission.Specialties_View,
                        EPermission.Doctors_View,
                        EPermission.MedicalRecords_View,
                        EPermission.MedicalRecords_Create,
                        EPermission.MedicalRecords_Update,
                        EPermission.Prescriptions_View,
                        EPermission.Appointments_View,
                        EPermission.Appointments_Update,
                        EPermission.Medicines_View,
                    }
                }
            };
        }

        private static List<Specialty> GetPreconfiguredSpecialties() => new()
        {
            new Specialty { Id = 1, Name = "Cardiology", Description = "Heart specialist" },
            new Specialty { Id = 2, Name = "ENT", Description = "Ear, Nose, and Throat specialist" },
        };

        private static List<Medicine> GetPreconfiguredMedicines() => new()
        {
            new Medicine { Id = 1, Name = "Aspirin", Description = "Pain reliever", Type = "Tablet" },
            new Medicine { Id = 2, Name = "Amoxicillin", Description = "Antibiotic", Type = "Capsule" },
            new Medicine { Id = 3, Name = "Lisinopril", Description = "Blood pressure", Type = "Tablet" }
        };

        private static List<Doctor> GetPreconfiguredDoctors(AppDbContext appContext)
        {
            var specialties = appContext.Specialties.ToList();
            var users = appContext.Users.ToList();

            return new List<Doctor>
            {
                new Doctor
                {
                    Id = 1,
                    FullName = "Dr. Abbas al-Numani",
                    SpecialtyID = specialties.FirstOrDefault(s => s.Name == "Cardiology")?.Id ?? 1,
                    Phone = "07712345678",
                    Email = "abbas@gmail.com",
                    UserID = users.FirstOrDefault(u => u.UserName == "abaas")?.Id ?? Guid.Empty
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

        private static List<Appointment> GetPreconfiguredAppointments(AppDbContext appContext)
        {
            var doctor = appContext.Doctors.FirstOrDefault();
            var patient = appContext.Patients.FirstOrDefault();

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

        private static List<Prescription> GetPreconfiguredPrescriptions(AppDbContext appContext)
        {
            var appointment = appContext.Appointments.FirstOrDefault();
            var doctor = appContext.Doctors.FirstOrDefault();
            var patient = appContext.Patients.FirstOrDefault();

            var medicines = appContext.Medicines.ToList();

            return new List<Prescription>
            {
                new Prescription
                {
                    Id = 1,
                    AppointmentID = appointment?.Id ?? 1,
                    DoctorID = doctor?.Id ?? 1,
                    PatientID = patient?.Id ?? 1,
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

        private static List<MedicalRecord> GetPreconfiguredMedicalRecords(AppDbContext appContext)
        {
            var doctor = appContext.Doctors.FirstOrDefault();
            var patient = appContext.Patients.FirstOrDefault();

            return new List<MedicalRecord>
            {
                new MedicalRecord
                {
                    Id = 1,
                    PatientID = patient?.Id ?? 1,
                    DoctorID = doctor?.Id ?? 1,
                    Diagnosis = "High blood pressure",
                    Notes = "Monitor blood pressure daily"
                }
            };
        }

        private static List<Invoice> GetPreconfiguredInvoices(AppDbContext appContext)
        {
            var patient = appContext.Patients.FirstOrDefault();
            var appointment = appContext.Appointments.FirstOrDefault();

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
                    PaymentMethod = PaymentMethod.Cash
                }
            };
        }
    }
}