using Microsoft.AspNetCore.Identity;

namespace Modules.Identity.Domain
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public Guid TenantId { get; set; }
    }
}
