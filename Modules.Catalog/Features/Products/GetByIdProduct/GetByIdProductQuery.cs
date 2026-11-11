using BuildingBlocks.Shared.Query;

namespace Modules.Catalog.Features.Products.GetByIdProduct
{
    public sealed record GetByIdProductDto(Guid Id, string Name, decimal Price);
    public sealed record GetByIdProductQuery(Guid Id) : IQuery<GetByIdProductDto>;
}
