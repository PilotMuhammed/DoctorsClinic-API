using DoctorsClinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoctorsClinic.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedicine> PrescriptionMedicines { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }


        // This class is used by EF Core Only when performing Add-Migration || Update-Database
        //public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        //{
        //    public AppDbContext CreateDbContext(string[] args)
        //    {
        //        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        //        optionsBuilder.UseNpgsql("Host=localhost;Database=DoctorsClinicDb;Username=postgres;Password=pilot");

        //        return new AppDbContext(optionsBuilder.Options);
        //    }
        //}
    }
}