using Microsoft.EntityFrameworkCore;
using mta.Models;
using Microsoft.Extensions.Logging;

namespace mta.Extensions;

public static class MultipleDatabaseExtensions
{
    public static IServiceCollection AddAndMigrateTenantDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        // Ajout d'un logger de base
        using IServiceScope scopeTenant = services.BuildServiceProvider().CreateScope();
        var logger = scopeTenant.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("MigrationLogger");

        TenantDbContext tenantDbContext = scopeTenant.ServiceProvider.GetRequiredService<TenantDbContext>();

        // Arrêt si la base centrale échoue
        try
        {
            if (tenantDbContext.Database.GetPendingMigrations().Any())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Applying BaseDb Migrations.");
                Console.ResetColor();
                tenantDbContext.Database.Migrate();
            }
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Échec de la migration de la base centrale. Arrêt de l’application.");
            // Notification externe (exemple fictif)
            EmailService.NotifyAdmin("Erreur migration base centrale", ex.ToString());
            throw;
        }

        List<Tenant> tenantsInDb = tenantDbContext.Tenants.ToList();
        string defaultConnectionString = configuration.GetConnectionString("DefaultConnection");

        // Collecte des erreurs pour reporting
        var errors = new List<string>();

        foreach (Tenant tenant in tenantsInDb)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(tenant.ConnectionString ?? defaultConnectionString);

                using var dbContext = new ApplicationDbContext(optionsBuilder.Options);

                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    logger.LogInformation($"Applying migrations for tenant {tenant.Name} ({tenant.Id})");
                    dbContext.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                var errorMsg = $"Tenant {tenant.Name} ({tenant.Id}): {ex.Message}";
                errors.Add(errorMsg);
                logger.LogError(ex, errorMsg);

                // Notification externe (exemple fictif)
                EmailService.NotifyAdmin($"Erreur migration tenant {tenant.Name}", ex.ToString());
            }
        }

        // Log/reporting global des erreurs
        if (errors.Any())
        {
            logger.LogWarning("Des erreurs de migration ont été rencontrées:\n" + string.Join("\n", errors));
        }

        return services;
    }
}

// Exemple fictif de service d'email
public static class EmailService
{
    public static void NotifyAdmin(string subject, string body)
    {
        // Implémentation réelle à remplacer par ton système de notification
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ALERTE ADMIN] {subject}\n{body}");
        Console.ResetColor();
    }
}