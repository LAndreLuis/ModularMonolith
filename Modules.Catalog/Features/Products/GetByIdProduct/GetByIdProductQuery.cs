using Shared.Query;

namespace Modules.Catalog.Features.Products.GetByIdProduct
{
    public record GetByIdProductDto(Guid Id, string Name, decimal Price);
    public record GetByIdProductQuery(Guid Id) : IQuery<GetByIdProductDto>;
}
