using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Data;

namespace Modules.Catalog.Features.Products.GetAllProduct
{
    public static class ProductCompiledQueries
    {
        public static readonly Func<
            CatalogDbContext,
            IAsyncEnumerable<GetAllProductDto>
        > GetAll =
            EF.CompileAsyncQuery(
                (CatalogDbContext context) =>
                    context.Products
                        .AsNoTracking()
                        .OrderBy(p => p.Id)
                .Take(100)
                        .Select(p => new GetAllProductDto(
                            p.Id,
                            p.Name,
                            p.Price
                        ))
            );
        
    }
}
