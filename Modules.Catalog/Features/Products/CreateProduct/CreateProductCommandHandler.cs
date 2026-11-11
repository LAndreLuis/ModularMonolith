using BuildingBlocks.Shared.Command;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Memory;
using Modules.Catalog.Data;
using Modules.Catalog.Domain;
using Modules.Catalog.Features.Products.GetAllProduct;

namespace Modules.Catalog.Features.Products.CreateProduct;

public sealed class CreateProductCommandHandler
    : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly CatalogDbContext _context;
    private readonly IMemoryCache _cache;
    private readonly IOutputCacheStore _outputCache;

    public CreateProductCommandHandler(
        CatalogDbContext context,
        IMemoryCache cache,
        IOutputCacheStore outputCache)
    {
        _context = context;
        _cache = cache;
        _outputCache = outputCache;
    }

    public async Task<Guid> HandleAsync(
        CreateProductCommand command,
        CancellationToken cancellationToken = default)
    {
        var product = new Product
        {
            Id = Guid.CreateVersion7(),
            Name = command.Name,
            Price = command.Price
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        _cache.Remove(GetAllProductQueryHandler.CacheKey);
        await _outputCache.EvictByTagAsync("products", cancellationToken);

        return product.Id;
    }
}