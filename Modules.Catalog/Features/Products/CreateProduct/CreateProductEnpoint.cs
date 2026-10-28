using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Catalog.Abstractions;

namespace Modules.Catalog.Features.Products.CreateProduct
{
    public static class CreateProductEnpoint
    {
        
        internal static void MapCreateProductEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (CreateProductCommand command, ICommandHandler<CreateProductCommand, Guid> handler) =>
            {
                var productId = await handler.HandleAsync(command);
                return Results.Created($"/products/{productId}", new { Id = productId });
            });
        }
    }
}
