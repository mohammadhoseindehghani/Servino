using app.Domain._common;
using app.Domain.CategoryAgg.Entities;
using app.Domain.CommentAgg.Entities;
using app.Domain.ExpertHomeServiceAgg.Entities;
using app.Domain.HomeServiceAgg.Entities;
using app.Domain.LocationAgg.Entities;
using app.Domain.RequestAgg.Entities;
using app.Domain.SuggestionAgg.Entities;
using app.Domain.UserAgg.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace app.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext(options)
{
    public new DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Expert> Experts { get; set; }
    public DbSet<Suggestion> Suggestions { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<RequestImage> RequestImages { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<HomeService> HomeServices { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ExpertHomeService> ExpertHomeServices { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entity in ChangeTracker.Entries<BaseEntity>())
        {
            entity.Entity.ModifiesAt = DateTime.Now;
            if (entity.State == EntityState.Added)
            {
                entity.Entity.CreatedAt = DateTime.Now;
            }
        }
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges()
    {
        foreach (var entity in ChangeTracker.Entries<BaseEntity>())
        {
            entity.Entity.ModifiesAt = DateTime.Now;
            if (entity.State == EntityState.Added)
            {
                entity.Entity.CreatedAt = DateTime.Now;
            }
        }
        return base.SaveChanges();
    }
}