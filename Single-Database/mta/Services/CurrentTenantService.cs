using mta.Models;

namespace mta.Services;

public class CurrentTenantService : ICurrentTenantService
{
    private readonly TenantDbContext _context;

    public CurrentTenantService(TenantDbContext context)
    {
        _context = context;
    }

    public string? TenantId { get; set; }

    public async Task<bool> SetTenant(string tenantId)
    {
        var tenantInfo = await _context.Tenants.FindAsync(tenantId);
        if (tenantInfo != null)
        {
            this.TenantId = tenantInfo.Id;
            return true;
        }
        
        throw new Exception("Tenant invalid");
        // return false;
    }
}
