using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Modules.Identity.Tenancy;

namespace Modules.Identity.Data
{
    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var tenantInfo = new AppTenantInfo
            {
                Id = "design-time-tenant",
                Identifier = "design-time"
            };

            var options = new DbContextOptionsBuilder<IdentityDbContext>()
                .UseSqlServer("Server=localhost\\SQL2014;Database=identity;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;")
                .Options;


            return MultiTenantDbContext.Create<IdentityDbContext, AppTenantInfo>(tenantInfo, options);
        }
    }
}