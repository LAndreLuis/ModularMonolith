using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Modules.Catalog.Tenancy; // ajuste conforme seu namespace real

namespace Modules.Catalog.Data
{
    public class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            // 1. Criar um TenantInfo fictício
            var tenantInfo = new AppTenantInfo
            {
                Id = "design-time-tenant-id",
                Identifier = "design-time"
            };

            // 2. Usar o método factory oficial do Finbuckle
            // Isso evita a necessidade de construir manualmente o IMultiTenantContextAccessor
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\SQL2014;Database=catalog;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;");

            return MultiTenantDbContext.Create<CatalogDbContext, AppTenantInfo>(tenantInfo, optionsBuilder.Options);
        }
    }
}