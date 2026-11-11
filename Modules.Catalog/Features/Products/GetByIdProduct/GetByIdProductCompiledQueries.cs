using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Data;

namespace Modules.Catalog.Features.Products.GetByIdProduct
{
    public static class ProductCompiledQueries
    {
        public static readonly Func<
            CatalogDbContext,
            Guid,
            IAsyncEnumerable<GetByIdProductDto>> GetById =
            EF.CompileAsyncQuery(
                (CatalogDbContext context, Guid id) =>
                    context.Products
                        .AsNoTracking()
                        .Where(p => p.Id == id)
                        .Select(p => new GetByIdProductDto(
                            p.Id,
                            p.Name,
                            p.Price)));
    }
}
