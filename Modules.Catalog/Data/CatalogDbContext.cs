using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Domain;

namespace Modules.Catalog.Data
{
    public sealed class CatalogDbContext : DbContext
    {
        public const string Schema = "Catalog";
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
         : base( options) { }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }


    }
}
