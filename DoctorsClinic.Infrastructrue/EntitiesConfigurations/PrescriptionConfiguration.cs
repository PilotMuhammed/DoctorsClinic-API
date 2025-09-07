using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DoctorsClinic.Domain.Entities;

namespace DoctorsClinic.Infrastructure.EntitiesConfigurations
{
    public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Notes)
                .HasMaxLength(300);

            builder.HasOne(p => p.Appointment)
                .WithMany(a => a.Prescriptions)
                .HasForeignKey(p => p.AppointmentID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Doctor)
                .WithMany(d => d.Prescriptions)
                .HasForeignKey(p => p.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Patient)
                .WithMany(pa => pa.Prescriptions)
                .HasForeignKey(p => p.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.PrescriptionMedicines)
                .WithOne(pm => pm.Prescription)
                .HasForeignKey(pm => pm.PrescriptionID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}