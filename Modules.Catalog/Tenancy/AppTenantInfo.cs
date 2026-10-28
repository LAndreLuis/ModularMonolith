
using Finbuckle.MultiTenant.Abstractions;

namespace Modules.Catalog.Tenancy
{

    public class AppTenantInfo : TenantInfo
    {
        // Se precisar de ConnectionString por tenant:
        public string? ConnectionString { get; set; }
    }
}
