using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Catalog.Abstractions;
using Modules.Catalog.Data;
using Modules.Catalog.Features.Products.CreateProduct;

namespace Modules.Catalog
{
    public static class CatalogModule
    {
        public static IEndpointRouteBuilder MapCatalogModule(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("api/catalog").RequireAuthorization();
            group.MapCreateProductEndpoint();
            return endpoints;
        }

        public static IServiceCollection AddCatalogModule(
    this IServiceCollection services,
    IConfiguration config)
        {
           
            services.AddDbContext<CatalogDbContext>(o =>
                o.UseSqlServer(config.GetConnectionString("Catalog")));


            services.AddScoped<ICommandHandler<
        CreateProductCommand, Guid>, CreateProductCommandHandler>();

            return services;
        }
    }
}
