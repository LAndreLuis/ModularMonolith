using BuildingBlocks.Shared.Query;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Catalog.Data;
using Modules.Catalog.Features.Products.CreateProduct;
using Modules.Catalog.Features.Products.DeleteProduct;
using Modules.Catalog.Features.Products.GetAllProduct;
using Modules.Catalog.Features.Products.GetByIdProduct;
using Modules.Catalog.Features.Products.UpdateProduct;
using Shared.Command;

namespace Modules.Catalog
{
    public static class CatalogModule
    {
        public static IEndpointRouteBuilder MapCatalogModule(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("api/catalog");
            group.MapCreateProductEndpoint();
            group.MapGetAllProductEndpoint();
            group.MapGetByIdProductEndpoint();
            group.MapUpdateProductEndpoint();
            group.MapDeleteProductEndpoint();
            return endpoints;
        }

        public static IServiceCollection AddCatalogModule(
    this IServiceCollection services,
    IConfiguration config)
        {
           
            services.AddDbContext<CatalogDbContext>(o =>
                o.UseSqlServer(config.GetConnectionString("Catalog")));

            services.AddCommandDispatcher();
            services.AddCommandHandlers(typeof(CatalogModule).Assembly);

            services.AddQueryDispatcher();
            services.AddQueryHandlers(typeof(CatalogModule).Assembly);



            return services;
        }
    }
}
