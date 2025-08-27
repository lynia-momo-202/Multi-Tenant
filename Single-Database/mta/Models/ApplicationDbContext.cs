using Microsoft.EntityFrameworkCore;

namespace mta.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
}
