using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DoctorsClinic.Domain.Entities;

namespace DoctorsClinic.Infrastructure.EntitiesConfigurations
{
    public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Diagnosis)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(m => m.Notes)
                .HasMaxLength(300);

            builder.HasOne(m => m.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(m => m.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Doctor)
                .WithMany(d => d.MedicalRecords)
                .HasForeignKey(m => m.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}