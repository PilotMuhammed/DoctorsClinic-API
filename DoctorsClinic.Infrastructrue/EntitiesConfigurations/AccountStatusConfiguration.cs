using DoctorsClinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorsClinic.Infrastructrue.EntitiesConfigurations
{
    public class AccountStatusConfiguration : IEntityTypeConfiguration<AccountStatus>
    {
        public void Configure(EntityTypeBuilder<AccountStatus> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(a => a.IsBlocked)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(a => a.IsLocked)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(a => a.FailedCount)
                .IsRequired();

            builder.Property(a => a.Reason)
                .HasMaxLength(300);

            builder.Property(a => a.LockDateTime);

            builder.HasOne(a => a.User)
                .WithOne(u => u.AccountStatus)
                .HasForeignKey<AccountStatus>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}