using Modules.Catalog.Data;
using Shared.Query;

namespace Modules.Catalog.Features.Products.GetByIdProduct
{
    public sealed class GetByIdProductQueryHandler : IQueryHandler<GetByIdProductQuery, GetByIdProductDto>
    {
        private readonly CatalogDbContext _context;
        public GetByIdProductQueryHandler(CatalogDbContext context)
        {
            _context = context;
        }
        public async Task<GetByIdProductDto> HandleAsync(GetByIdProductQuery query, CancellationToken ct = default)
        {
            var product = await _context.Products.FindAsync(query.Id, ct);
            if (product == null)
                throw new InvalidOperationException("Product not found");

            return new GetByIdProductDto(product.Id, product.Name, product.Price);
        }
    }
}
