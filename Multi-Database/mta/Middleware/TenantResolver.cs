using mta.Services;

namespace mta.Middleware;

public class TenantResolver
{
    private readonly RequestDelegate _next;

    public TenantResolver(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICurrentTenantService currentTenantService)
    {
        /*
            // Extract tenant information from the request (e.g., from headers, query parameters, etc.)
            var tenantId = context.Request.Headers["X-Tenant-ID"].FirstOrDefault();

            if (!string.IsNullOrEmpty(tenantId))
            {
                // Store the tenant information in the HttpContext for later use
                context.Items["TenantId"] = tenantId;
            }

            // Call the next middleware in the pipeline
            await _next(context);
        */

        context.Request.Headers.TryGetValue("tenant", out var tenantFromHeader);
        if (!string.IsNullOrEmpty(tenantFromHeader))
        {
            //set tenant id in scoped service
            await currentTenantService.SetTenant(tenantFromHeader);
        }

        await _next(context);
    }
}
