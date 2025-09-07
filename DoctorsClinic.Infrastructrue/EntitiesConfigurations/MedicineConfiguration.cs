using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DoctorsClinic.Domain.Entities;

namespace DoctorsClinic.Infrastructure.EntitiesConfigurations
{
    public class MedicineConfiguration : IEntityTypeConfiguration<Medicine>
    {
        public void Configure(EntityTypeBuilder<Medicine> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.Type)
                .HasMaxLength(50);

            builder.HasMany(m => m.PrescriptionMedicines)
                .WithOne(pm => pm.Medicine)
                .HasForeignKey(pm => pm.MedicineID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}