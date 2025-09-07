using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DoctorsClinic.Domain.Entities;
 
namespace DoctorsClinic.Infrastructure.EntitiesConfigurations
{
    public class PrescriptionMedicineConfiguration : IEntityTypeConfiguration<PrescriptionMedicine>
    {
        public void Configure(EntityTypeBuilder<PrescriptionMedicine> builder)
        {
            builder.HasKey(pm => pm.Id);

            builder.Property(pm => pm.Dose)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(pm => pm.Duration)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(pm => pm.Instructions)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(pm => pm.Prescription)
                .WithMany(p => p.PrescriptionMedicines)
                .HasForeignKey(pm => pm.PrescriptionID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pm => pm.Medicine)
                .WithMany(m => m.PrescriptionMedicines)
                .HasForeignKey(pm => pm.MedicineID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}