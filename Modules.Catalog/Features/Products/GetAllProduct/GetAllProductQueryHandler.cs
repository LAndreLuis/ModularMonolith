using BuildingBlocks.Shared.Query;
using Microsoft.Extensions.Caching.Memory;
using Modules.Catalog.Data;

namespace Modules.Catalog.Features.Products.GetAllProduct;

public sealed class GetAllProductQueryHandler
    : IQueryHandler<GetAllProductQuery, IEnumerable<GetAllProductDto>>
{
    private readonly CatalogDbContext _context;
    private readonly IMemoryCache _cache;

    internal const string CacheKey = "catalog:products:all";

    private static readonly SemaphoreSlim CacheLock = new(1, 1);

    public GetAllProductQueryHandler(CatalogDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<IEnumerable<GetAllProductDto>> HandleAsync(
        GetAllProductQuery query,
        CancellationToken ct = default)
    {
        if (_cache.TryGetValue(CacheKey, out IReadOnlyList<GetAllProductDto>? cached)
            && cached is not null)
        {
            return cached;
        }

        await CacheLock.WaitAsync(ct);
        try
        {
            if (_cache.TryGetValue(CacheKey, out cached) && cached is not null)
            {
                return cached;
            }

            
            using var loadCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            var products = new List<GetAllProductDto>();
            await foreach (var product in ProductCompiledQueries
                .GetAll(_context)
                .WithCancellation(loadCts.Token))
            {
                products.Add(product);
            }

            IReadOnlyList<GetAllProductDto> result = products.AsReadOnly();

            _cache.Set(
                CacheKey,
                result,
                new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1)));

            return result;
        }
        finally
        {
            CacheLock.Release();
        }
    }
}