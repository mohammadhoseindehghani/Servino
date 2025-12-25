using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Entity;
using Servino.Domain.Core.CommentAgg.Entity;
using Servino.Domain.Core.ExpertSkillAgg.Entity;
using Servino.Domain.Core.HomeServiceAgg.Entity;
using Servino.Domain.Core.LocationAgg.Entity;
using Servino.Domain.Core.RequestAgg.Entity;
using Servino.Domain.Core.SuggestionAgg.Entity;
using Servino.Domain.Core.UserAgg.Entity;

namespace Servino.Infa.Db.SqlServer.EfCore.DbContexts;

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
    public DbSet<ExpertSkill> ExpertSkills { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        builder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        builder.Entity<Customer>().HasQueryFilter(c => !c.IsDeleted);
        builder.Entity<Expert>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Admin>().HasQueryFilter(a => !a.IsDeleted);
        builder.Entity<HomeService>().HasQueryFilter(h => !h.IsDeleted);
        builder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
        builder.Entity<ExpertSkill>().HasQueryFilter(s => !s.IsDeleted);
        builder.Entity<Request>().HasQueryFilter(r => !r.IsDeleted);
        builder.Entity<Suggestion>().HasQueryFilter(s => !s.IsDeleted);
        builder.Entity<Comment>().HasQueryFilter(c => !c.IsDeleted);
        builder.Entity<Province>().HasQueryFilter(p => !p.IsDeleted);
        builder.Entity<City>().HasQueryFilter(c => !c.IsDeleted);
    }

    public override int SaveChanges()
    {
        SetAuditDates();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        SetAuditDates();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditDates()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity.CreatedAt == default)
                    entry.Entity.CreatedAt = DateTime.Now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.Now;
            }
        }
    }
}