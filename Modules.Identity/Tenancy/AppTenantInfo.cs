
using Finbuckle.MultiTenant.Abstractions;

namespace Modules.Identity.Tenancy
{

    public class AppTenantInfo : TenantInfo
    {
        // Se precisar de ConnectionString por tenant:
        public string? ConnectionString { get; set; }
    }
}
