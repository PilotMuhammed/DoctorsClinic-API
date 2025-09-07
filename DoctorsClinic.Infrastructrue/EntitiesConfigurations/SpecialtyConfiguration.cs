using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DoctorsClinic.Domain.Entities;

namespace DoctorsClinic.Infrastructure.EntitiesConfigurations
{
    public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Description)
                .HasMaxLength(300);

            builder.HasMany(s => s.Doctors)
                .WithOne(d => d.Specialty)
                .HasForeignKey(d => d.SpecialtyID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}