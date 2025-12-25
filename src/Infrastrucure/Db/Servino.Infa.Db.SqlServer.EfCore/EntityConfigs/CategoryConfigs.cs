using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servino.Domain.Core.CategoryAgg.Entity;

namespace Servino.Infa.Db.SqlServer.EfCore.EntityConfigs;

public class CategoryConfigs : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.ImagePath)
            .HasMaxLength(300);

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);

        builder.Property(c => c.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder
            .HasOne(c => c.Parent)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(c => c.Services)
            .WithOne(s => s.Category)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.ParentId);

        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}