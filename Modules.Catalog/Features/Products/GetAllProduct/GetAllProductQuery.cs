using BuildingBlocks.Shared.Query;

namespace Modules.Catalog.Features.Products.GetAllProduct
{

    public sealed record GetAllProductDto(Guid Id, string Name, decimal Price);
    public sealed record GetAllProductQuery : IQuery<IEnumerable<GetAllProductDto>>;
    
}
