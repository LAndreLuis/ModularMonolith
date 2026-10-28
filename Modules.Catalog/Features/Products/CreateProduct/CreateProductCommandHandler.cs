using Modules.Catalog.Data;
using Modules.Catalog.Domain;
using Modules.Catalog.Abstractions;

namespace Modules.Catalog.Features.Products.CreateProduct
{
    public sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
    {
        private CatalogDbContext _context;

        public CreateProductCommandHandler(CatalogDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Price = command.Price
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
            return product.Id;
        }
    }
}
