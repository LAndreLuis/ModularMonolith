using BuildingBlocks.Shared.Command;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Command;

namespace Modules.Catalog.Features.Products.UpdateProduct
{
    public static class UpdateProductEndpoint
    {
        internal static void MapUpdateProductEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("products/{id:guid}", async (Guid id, UpdateProductDto dto, ICommandDispatcher dispatcher) =>
            {
                if (id == Guid.Empty)
                {
                    return Results.BadRequest("Id in the URL is invalid.");
                }
                var result = await dispatcher.SendAsync(new UpdateProductCommand(id, dto.Name, dto.Price));
                return Results.Ok(result);
            });
            
        }
    }
}
