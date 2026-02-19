using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servino.Domain.Core.UserAgg.Entity;

namespace Servino.Infa.Db.SqlServer.EfCore.EntityConfigs;

public class AdminConfigs : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable("Admins");

        builder.HasKey(a => a.Id);

        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => a.UserId)
            .IsUnique();

        builder.Property(a => a.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}