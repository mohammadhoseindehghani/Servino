using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servino.Domain.Core.ExpertSkillAgg.Entity;

namespace Servino.Infa.Db.SqlServer.EfCore.EntityConfigs;

public class ExpertSkillConfigs : IEntityTypeConfiguration<ExpertSkill>
{
    public void Configure(EntityTypeBuilder<ExpertSkill> builder)
    {
        builder.ToTable("ExpertSkills");

        builder.HasKey(es => new { es.ExpertId, es.HomeServiceId });

        builder.Property(es => es.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(es => es.Expert)
            .WithMany(e => e.Skills)
            .HasForeignKey(es => es.ExpertId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(es => es.HomeService)
            .WithMany(h => h.ExpertSkills)
            .HasForeignKey(es => es.HomeServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(es => !es.IsDeleted);
    }
}