using static Modules.Catalog.ICommands.ICommands;

namespace Modules.Catalog.Features.Products.CreateProduct
{
    public record CreateProductCommand(string Name, decimal Price) : ICommand<Guid>
    {
        
    }
}
