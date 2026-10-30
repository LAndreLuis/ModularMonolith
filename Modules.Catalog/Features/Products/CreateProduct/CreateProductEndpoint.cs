using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Command;

namespace Modules.Catalog.Features.Products.CreateProduct
{
    public static class CreateProductEndpoint
    {
        
        internal static void MapCreateProductEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (CreateProductCommand command, ICommandDispatcher dispatcher, CancellationToken ct) =>
            {
                var productId = await dispatcher.SendAsync(command, ct);
                return Results.Created($"/api/catalog/products/{productId}", new { Id = productId });
            });
        }
    }
}
