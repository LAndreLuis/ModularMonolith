using Shared.Query;

namespace Modules.Catalog.Features.Products.GetAllProduct
{

    public record GetAllProductDto(Guid Id, string Name, decimal Price);
    public record GetAllProductQuery : IQuery<IEnumerable<GetAllProductDto>>;
    
}
