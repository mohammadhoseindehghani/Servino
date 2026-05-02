using app.Domain.ExpertHomeServiceAgg.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app.Persistence.EntityConfigs;

public class ExpertHomeServiceConfigs : IEntityTypeConfiguration<ExpertHomeService>
{
    public void Configure(EntityTypeBuilder<ExpertHomeService> builder)
    {
        builder.ToTable("ExpertHomeServices");

        builder.HasKey(es => new { es.ExpertId, es.HomeServiceId });

        builder.Property(es => es.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(es => es.Expert)
            .WithMany(e => e.ExpertHomeServices)
            .HasForeignKey(es => es.ExpertId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(es => es.HomeService)
            .WithMany(h => h.ExpertHomeServices)
            .HasForeignKey(es => es.HomeServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(es => !es.IsDeleted);
    }
}