using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Modules.Catalog.Features.Products.CreateProduct;

namespace Modules.Catalog
{
    public static class CatalogModule
    {
        public static IEndpointRouteBuilder MapCatalogModule(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("api/catalog");
            group.MapCreateProductEndpoint();
            return endpoints;
        }
    }
}
