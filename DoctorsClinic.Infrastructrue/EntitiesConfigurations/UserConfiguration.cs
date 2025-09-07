using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DoctorsClinic.Domain.Entities;

namespace DoctorsClinic.Infrastructure.EntitiesConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.Role)
                .IsRequired()
                .HasConversion<int>();

            builder.HasOne(u => u.Doctor)
                .WithOne(d => d.User)
                .HasForeignKey<User>(u => u.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}