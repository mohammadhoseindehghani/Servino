using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servino.Domain.Core.RequestAgg.Entity;

namespace Servino.Infa.Db.SqlServer.EfCore.EntityConfigs;

public class RequestImageConfigs : IEntityTypeConfiguration<RequestImage>
{
    public void Configure(EntityTypeBuilder<RequestImage> builder)
    {
        builder.ToTable("RequestImages");

        builder.HasKey(ri => ri.Id);

        builder.Property(ri => ri.ImagePath)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ri => ri.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(ri => ri.Request)
            .WithMany(r => r.Images)
            .HasForeignKey(ri => ri.RequestId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}