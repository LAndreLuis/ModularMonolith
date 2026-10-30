using BuildingBlocks.Shared.Query;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


namespace Modules.Catalog.Features.Products.GetAllProduct
{
    public static class GetAllProductEndpoint
    {
        internal static void MapGetAllProductEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (IQueryDispatcher dispatcher, CancellationToken ct) =>
            {
                var products = await dispatcher.SendAsync(new GetAllProductQuery(), ct);
                return Results.Ok(products);
            });
            
        }
    }
}
