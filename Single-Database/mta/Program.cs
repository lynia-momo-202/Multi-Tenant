using Microsoft.EntityFrameworkCore;
using mta.Middleware;
using mta.Models;
using mta.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationDbContext>(
                    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<TenantDbContext>(
                    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();
builder.Services.AddTransient<IProductService, ProductService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<TenantResolver>();

app.MapControllers();

app.Run();
