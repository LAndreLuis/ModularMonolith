using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Identity.Abstractions;
using Modules.Identity.Domain;
using Modules.Identity.Features.Identity.CreateLogin;
using Microsoft.EntityFrameworkCore;


namespace Modules.Identity
{
    public static class IdentityModule
    {
        public static IEndpointRouteBuilder MapIdentityModule(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("api/identity");
            group.MapCreateLoginEndpoint();
            return endpoints;
        }

        public static IServiceCollection AddIdentityModule(
            this IServiceCollection services,
            IConfiguration config)
        {
            // DbContext + Identity + JWT + handler
            services.AddDbContext<IdentityDbContext>(o =>
                o.UseSqlServer(config.GetConnectionString("Identity")));


        
            services.AddScoped<
                ICommandHandler<CreateLoginCommand, LoginResult>,
                CreateLoginCommandHandler>();

            return services;
        }
    }
}