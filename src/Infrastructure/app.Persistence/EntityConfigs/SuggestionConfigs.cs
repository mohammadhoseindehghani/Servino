using app.Domain.SuggestionAgg.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app.Persistence.EntityConfigs;

public class SuggestionConfigs : IEntityTypeConfiguration<Suggestion>
{
    public void Configure(EntityTypeBuilder<Suggestion> builder)
    {
        builder.ToTable("Suggestions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Note)
            .HasMaxLength(500); 

        builder.Property(s => s.SuggestedPrice)
            .IsRequired()
            .HasPrecision(18, 0);

        builder.Property(s => s.SuggestedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(s => s.EstimatedDurationHours)
            .IsRequired();

        builder.Property(s => s.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(s => s.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(s => s.Request)
            .WithMany(r => r.Suggestions)
            .HasForeignKey(s => s.RequestId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Expert)
            .WithMany(e => e.Suggestions) 
            .HasForeignKey(s => s.ExpertId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(s => !s.IsDeleted);

        builder.HasIndex(s => s.RequestId);
        builder.HasIndex(s => s.ExpertId);
        builder.HasIndex(s => s.Status);
    }
}