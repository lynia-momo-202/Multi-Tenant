using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mta.DTOs;
using mta.Services.TenantService;

namespace mta.Controllers
{
    [Route("api/[controller]")]
[ApiController]
public class TenantsController : ControllerBase
{
    private readonly ITenantService _tenantService;

    public TenantsController(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpGet("get")]
    public IActionResult Get()
    {
        var tenants = _tenantService.GetTenants();
        return Ok(tenants);
    }
    
    // Create a new tenant
        [HttpPost("create")]
    public IActionResult CreateTenant(CreateTenantRequest request)
    {
        var result = _tenantService.CreateTenant(request);
        return Ok(result);
    }
}
}
