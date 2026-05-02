using app.Domain.CommentAgg.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app.Persistence.EntityConfigs;

public class CommentConfigs : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Text)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.Rating)
            .IsRequired()
            .HasConversion<int>();

        builder.HasCheckConstraint(
            "CK_Comments_Rating",
            "[Rating] BETWEEN 1 AND 5"
        );

        builder.Property(c => c.IsApproved)
            .HasDefaultValue(false);

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(c => c.Customer)
            .WithMany(cu => cu.Comments)
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Expert)
            .WithMany(e => e.CommentsReceived)
            .HasForeignKey(c => c.ExpertId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Request)
            .WithOne(r => r.Comment)
            .HasForeignKey<Comment>(c => c.RequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.CustomerId);
        builder.HasIndex(c => c.ExpertId);
        builder.HasIndex(c => c.RequestId)
            .IsUnique();

        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}