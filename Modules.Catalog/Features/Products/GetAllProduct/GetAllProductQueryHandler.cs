using BuildingBlocks.Shared.Query;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Data;

namespace Modules.Catalog.Features.Products.GetAllProduct
{
    public sealed class GetAllProductQueryHandler : IQueryHandler<GetAllProductQuery, IEnumerable<GetAllProductDto>>
    {
        private readonly CatalogDbContext _context;

        public GetAllProductQueryHandler(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetAllProductDto>> HandleAsync(GetAllProductQuery query, CancellationToken ct = default)
        {

            return await _context.Products
                //.Where(p => p.TenantId == query.TenantId)
            .AsNoTracking()
            .Select(p => new GetAllProductDto(p.Id, p.Name, p.Price))
            .ToListAsync(ct);
        }
    }
}
