using BuildingBlocks.Shared.Command;
using Modules.Catalog.Data;

namespace Modules.Catalog.Features.Products.DeleteProduct
{
    public sealed class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, Guid>
    {
        private readonly CatalogDbContext _dbContext;
        public DeleteProductCommandHandler(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> HandleAsync(DeleteProductCommand command, CancellationToken ct = default)
        {
            var product = await _dbContext.Products.FindAsync(command.Id, ct);
            if (product == null)
                throw new InvalidOperationException("Product not found");

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync(ct);
            return product.Id;
        }
    }
}
