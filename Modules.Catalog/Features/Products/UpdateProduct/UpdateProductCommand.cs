using BuildingBlocks.Shared.Command;

namespace Modules.Catalog.Features.Products.UpdateProduct
{
    public record UpdateProductDto(string Name, decimal Price);
    public record UpdateProductCommand(Guid Id, string Name, decimal Price) : ICommand<UpdateProductDto>;
}
