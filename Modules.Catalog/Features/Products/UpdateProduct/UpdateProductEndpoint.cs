using BuildingBlocks.Shared.Command;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Command;

namespace Modules.Catalog.Features.Products.UpdateProduct;

public static class UpdateProductEndpoint
{
    internal static void MapUpdateProductEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPut("products/{id:guid}", async (
            Guid id,
            UpdateProductDto dto,
            ICommandDispatcher dispatcher,
            CancellationToken ct) =>
        {
            if (id == Guid.Empty)
                return Results.BadRequest("Id in the URL is invalid.");

            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Price < 0)
                return Results.BadRequest("Name is required and Price must be >= 0.");

            try
            {
                var result = await dispatcher.SendAsync(
                    new UpdateProductCommand(id, dto.Name, dto.Price), ct);
                return Results.Ok(result);
            }
            catch (ProductNotFoundException)
            {
                return Results.NotFound();
            }
        });
    }
}