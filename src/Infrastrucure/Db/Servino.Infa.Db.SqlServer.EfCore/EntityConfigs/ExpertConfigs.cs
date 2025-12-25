using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servino.Domain.Core.UserAgg.Entity;

namespace Servino.Infa.Db.SqlServer.EfCore.EntityConfigs;

public class ExpertConfigs : IEntityTypeConfiguration<Expert>
{
    public void Configure(EntityTypeBuilder<Expert> builder)
    {
        builder.ToTable("Experts");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.UserId)
            .IsUnique();

        builder.Property(e => e.Bio)
            .HasMaxLength(500);

        builder.Property(e => e.Address)
            .HasMaxLength(300);

        builder.Property(e => e.BankCardNumber)
            .HasMaxLength(16);

        builder.Property(e => e.ShebaNumber)
            .HasMaxLength(26);

        builder.Property(e => e.AverageScore)
            .HasPrecision(3, 2);

        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(e => e.User)
            .WithOne(u => u.Expert)
            .HasForeignKey<Expert>(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.CommentsReceived)
            .WithOne(c => c.Expert)
            .HasForeignKey(c => c.ExpertId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}