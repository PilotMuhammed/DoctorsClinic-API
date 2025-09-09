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

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(128);

            builder.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.UserRoleID);

            builder.HasMany(u => u.Permissions)
                .WithOne(up => up.User)
                .HasForeignKey(up => up.UserID);

            builder.HasOne(u => u.Doctor)
                .WithOne(d => d.User)
                .HasForeignKey<User>(u => u.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}