using System.Text;
using Finbuckle.MultiTenant.AspNetCore.Extensions;
using Finbuckle.MultiTenant.EntityFrameworkCore.Extensions;
using Finbuckle.MultiTenant.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Modules.Catalog;
using Modules.Catalog.Data;
using Modules.Catalog.Tenancy;
using Modules.Identity;
using Modules.Identity.Data;
using Modules.Identity.Domain;
using Modules.Identity.Services;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddCatalogModule(builder.Configuration);


builder.Services.AddMultiTenant<AppTenantInfo>()
    .WithClaimStrategy("tenant_id", JwtBearerDefaults.AuthenticationScheme)
    .WithInMemoryStore(options =>
    {
        options.Tenants.Add(new AppTenantInfo
        {
            Id = "11111111-1111-1111-1111-111111111111",
            Identifier = "tenant-a",
            Name = "Tenant A"
        });
    });

var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseMultiTenant();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    var tenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    if (await userManager.FindByEmailAsync("admin@tenant-a.com") is null)
    {
        var user = new AppUser
        {
            Email = "admin@tenant-a.com",
            TenantId = tenantId,
            PasswordHash = PasswordHasherService.HashPassword("Senha123!")
        };
        await userManager.CreateAsync(user, "Senha123!");
    }
}

app.MapControllers();

app.MapCatalogModule();
app.MapIdentityModule();

app.Run();

