using app.Domain.UserAgg.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app.Persistence.EntityConfigs;

public class UserConfigs : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.MobileNumber)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.ProfileImagePath)
            .HasMaxLength(200); 

        builder.Property(u => u.Balance)
            .HasPrecision(18, 0)
            .HasDefaultValue(0);

        builder.Property(u => u.IsActive)
            .HasDefaultValue(true);

        builder.Property(u => u.IdentityId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(u => u.UpdatedAt);

        builder.HasOne(u => u.Customer)
            .WithOne(c => c.User)
            .HasForeignKey<Customer>(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Expert)
            .WithOne(e => e.User)
            .HasForeignKey<Expert>(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Admin)
            .WithOne(a => a.User)
            .HasForeignKey<Admin>(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.City)
            .WithMany(c => c.Users)
            .HasForeignKey(u => u.CityId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.MobileNumber).IsUnique();
        builder.HasIndex(u => u.IdentityId).IsUnique();

        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}