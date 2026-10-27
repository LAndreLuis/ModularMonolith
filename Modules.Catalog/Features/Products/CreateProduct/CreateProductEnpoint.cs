using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using static Modules.Catalog.ICommands.ICommands;

namespace Modules.Catalog.Features.Products.CreateProduct
{
    public static class CreateProductEnpoint
    {
        
        internal static void MapCreateProductEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/products", async (CreateProductCommand command, ICommandHandler<CreateProductCommand, Guid> handler) =>
            {
                var productId = await handler.HandleAsync(command);
                return Results.Created($"/api/products/{productId}", new { Id = productId });
            });
        }
    }
}
