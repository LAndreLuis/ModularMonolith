using BuildingBlocks.Shared.Command;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Modules.Catalog.Data;
using Modules.Catalog.Features.Products.GetAllProduct;

namespace Modules.Catalog.Features.Products.UpdateProduct;

public sealed class UpdateProductCommandHandler
    : ICommandHandler<UpdateProductCommand, UpdateProductDto>
{
    private readonly CatalogDbContext _context;
    private readonly IMemoryCache _cache;
    private readonly IOutputCacheStore _outputCache;

    public UpdateProductCommandHandler(
        CatalogDbContext context,
        IMemoryCache cache,
        IOutputCacheStore outputCache)
    {
        _context = context;
        _cache = cache;
        _outputCache = outputCache;
    }

    public async Task<UpdateProductDto> HandleAsync(
        UpdateProductCommand command,
        CancellationToken ct = default)
    {
        // Um único UPDATE no banco: sem SELECT prévio, sem change tracker.
        var rows = await _context.Products
            .Where(p => p.Id == command.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.Name, command.Name)
                .SetProperty(p => p.Price, command.Price), ct);

        if (rows == 0)
            throw new ProductNotFoundException(command.Id);

        // Primeiro o cache de dentro, depois o de fora.
        _cache.Remove(GetAllProductQueryHandler.CacheKey);
        await _outputCache.EvictByTagAsync("products", ct);

        return new UpdateProductDto(command.Name, command.Price);
    }
}