using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Domain;

namespace Modules.Catalog.Data
{
    public sealed class CatalogDbContext : MultiTenantDbContext
    {
        public const string Schema = "Catalog";
        public CatalogDbContext(
         IMultiTenantContextAccessor multiTenantContextAccessor,
         DbContextOptions<CatalogDbContext> options)
         : base(multiTenantContextAccessor, options) { }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }


    }
}
