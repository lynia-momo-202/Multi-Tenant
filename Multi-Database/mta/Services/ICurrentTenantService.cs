namespace mta.Services;

public interface ICurrentTenantService
{
    public string TenantId { get; set; }
    public string ConnectionString { get; set; }

    public Task<bool> SetTenant(string tenantId);

}
