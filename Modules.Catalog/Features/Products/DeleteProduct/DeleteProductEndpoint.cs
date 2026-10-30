using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Command;

namespace Modules.Catalog.Features.Products.DeleteProduct
{
    public static class DeleteProductEndpoint
    {
        internal static void MapDeleteProductEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id:guid}", async (Guid id, ICommandDispatcher dispatcher, CancellationToken ct) =>
            {
                var result = await dispatcher.SendAsync(new DeleteProductCommand(id), ct);
                return Results.Ok(result);
            });
        }
    }
}
