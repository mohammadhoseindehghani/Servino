using app.Domain.RequestAgg.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app.Persistence.EntityConfigs;

public class RequestConfigs : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.ToTable("Requests");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(r => r.Address)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(r => r.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(r => r.City)
            .WithMany(c => c.Requests)
            .HasForeignKey(r => r.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Customer)
            .WithMany(c => c.Requests)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.HomeService)
            .WithMany(h => h.Requests)
            .HasForeignKey(r => r.HomeServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.WinnerSuggestion)
            .WithMany()
            .HasForeignKey(r => r.WinnerSuggestionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Comment)
            .WithOne(c => c.Request)
            .HasForeignKey<Comment>(c => c.RequestId);

        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}