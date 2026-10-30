using BuildingBlocks.Shared.Query;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Modules.Catalog.Features.Products.GetByIdProduct
{
    public static class GetByIdProductEndpoint
    {

        internal static void MapGetByIdProductEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id:guid}", async (Guid id, IQueryDispatcher dispatcher, CancellationToken ct) =>
            {
                var result = await dispatcher.SendAsync(new GetByIdProductQuery(id), ct);
                return Results.Ok(result);
            });
        }
    }
}
