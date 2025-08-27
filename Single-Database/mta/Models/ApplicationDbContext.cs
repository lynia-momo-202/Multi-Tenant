using Microsoft.EntityFrameworkCore;
using mta.Services;

namespace mta.Models;

public class ApplicationDbContext : DbContext
{
    private readonly ICurrentTenantService _currentTenantService;
    public string currentTenantId { get; set; }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentTenantService currentTenantService) : base(options)
    {
        _currentTenantService = currentTenantService;
        currentTenantId = _currentTenantService.TenantId;
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Tenant> Tenants { get; set; }

    //on app startup
    protected override void OnModelCreating(ModelBuilder Builder)
    {
        Builder.Entity<Product>().HasQueryFilter(a => a.TenantId == currentTenantId);
    }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                case EntityState.Modified:
                    entry.Entity.TenantId = currentTenantId;
                    break;
            }
        }

        var result = base.SaveChanges();
        return result;
    }
}
