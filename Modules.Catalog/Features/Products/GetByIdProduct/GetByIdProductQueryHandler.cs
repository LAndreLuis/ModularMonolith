using BuildingBlocks.Shared.Query;
using Modules.Catalog.Data;

namespace Modules.Catalog.Features.Products.GetByIdProduct;

public sealed class GetByIdProductQueryHandler
    : IQueryHandler<GetByIdProductQuery, GetByIdProductDto>
{
    private readonly CatalogDbContext _context;

    public GetByIdProductQueryHandler(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<GetByIdProductDto> HandleAsync(
        GetByIdProductQuery query,
        CancellationToken ct = default)
    {
        await foreach (var product in ProductCompiledQueries
            .GetById(_context, query.Id)
            .WithCancellation(ct))
        {
            return product;
        }

        throw new ProductNotFoundException(query.Id);
    }
}