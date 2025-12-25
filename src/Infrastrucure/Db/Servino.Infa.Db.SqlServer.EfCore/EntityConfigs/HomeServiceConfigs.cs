using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servino.Domain.Core.HomeServiceAgg.Entity;

namespace Servino.Infa.Db.SqlServer.EfCore.EntityConfigs;

public class HomeServiceConfigs : IEntityTypeConfiguration<HomeService>
{
    public void Configure(EntityTypeBuilder<HomeService> builder)
    {
        builder.ToTable("HomeServices");

        builder.HasKey(hs => hs.Id);

        builder.Property(hs => hs.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(hs => hs.ShortDescription)
            .HasMaxLength(200);

        builder.Property(hs => hs.ImagePath)
            .HasMaxLength(200);

        builder.Property(hs => hs.BasePrice)
            .IsRequired()
            .HasPrecision(18, 0);

        builder.Property(hs => hs.IsActive)
            .HasDefaultValue(true);

        builder.Property(hs => hs.VisitCount)
            .HasDefaultValue(0);

        builder.Property(hs => hs.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(hs => hs.Category)
            .WithMany(c => c.Services)
            .HasForeignKey(hs => hs.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(hs => hs.ExpertSkills)
            .WithOne(es => es.HomeService)
            .HasForeignKey(es => es.HomeServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(hs => hs.Requests)
            .WithOne(r => r.HomeService)
            .HasForeignKey(r => r.HomeServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(hs => hs.CategoryId);
        builder.HasIndex(hs => hs.IsActive);

        builder.HasQueryFilter(hs => !hs.IsDeleted);
    }
}