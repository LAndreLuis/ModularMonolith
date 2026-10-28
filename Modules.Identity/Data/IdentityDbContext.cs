using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Modules.Identity.Domain;

namespace Modules.Identity.Data
{
    public sealed class IdentityDbContext : MultiTenantDbContext
    {
        public const string Schema = "Identity";

        public IdentityDbContext(
            IMultiTenantContextAccessor multiTenantContextAccessor,
            DbContextOptions<IdentityDbContext> options)
            : base(multiTenantContextAccessor, options) { }

        public DbSet<AppUser> Users => Set<AppUser>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.Entity<AppUser>(builder =>
            {
                builder.ToTable("Users");
                builder.HasKey(u => u.Id);
                builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
                builder.Property(u => u.PasswordHash).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}