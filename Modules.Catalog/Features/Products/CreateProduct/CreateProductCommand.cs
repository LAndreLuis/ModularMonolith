using BuildingBlocks.Shared.Command;

namespace Modules.Catalog.Features.Products.CreateProduct
{
    public record CreateProductCommand(string Name, decimal Price) : ICommand<Guid>;
}