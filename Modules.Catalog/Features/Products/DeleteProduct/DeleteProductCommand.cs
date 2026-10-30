using BuildingBlocks.Shared.Command;

namespace Modules.Catalog.Features.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<Guid>;
}
