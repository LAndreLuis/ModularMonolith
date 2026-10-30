using BuildingBlocks.Shared.Command;
using Modules.Catalog.Data;

namespace Modules.Catalog.Features.Products.UpdateProduct
{

    public sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, UpdateProductDto>
    {
        private readonly CatalogDbContext _context;
        public UpdateProductCommandHandler(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateProductDto> HandleAsync(UpdateProductCommand command, CancellationToken ct = default)
        {
            var product = await _context.Products.FindAsync(command.Id, ct);
            if (product == null)
                throw new InvalidOperationException("Product not found");

            product.Name = command.Name;
            product.Price = command.Price;

            await _context.SaveChangesAsync(ct);
            return new UpdateProductDto(product.Name, product.Price);
        }
    }
}
