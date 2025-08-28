using mta.DTOs;
using mta.Models;

namespace mta.Services.TenantService;

public interface ITenantService
{
    IEnumerable<Tenant> GetTenants();
    Tenant CreateTenant(CreateTenantRequest request);
}
