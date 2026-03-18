// GOOD: HybridCache — Built-in Stampede Protection
// Only one request hits the database when cache expires.
// The rest wait and share the same result automatically.

using Microsoft.Extensions.Caching.Hybrid;

public class ProductServiceWithHybridCache
{
    private readonly HybridCache _cache;
    private readonly IProductRepository _repository;
    private static readonly HybridCacheEntryOptions _cacheOptions = new()
    {
        Expiration = TimeSpan.FromMinutes(5),
        LocalCacheExpiration = TimeSpan.FromMinutes(1)
    };

    public ProductServiceWithHybridCache(HybridCache cache, IProductRepository repository)
    {
        _cache = cache;
        _repository = repository;
    }

    public async Task<Product?> GetProductAsync(int productId, CancellationToken ct = default)
    {
        var key = $"product_{productId}";

        // HybridCache handles stampede protection automatically.
        // If 100 requests arrive simultaneously and cache is empty:
        // → Only 1 request fetches from DB
        // → Other 99 wait and receive the same result
        return await _cache.GetOrCreateAsync(
            key,
            async cancel => await _repository.GetByIdAsync(productId),
            _cacheOptions,
            cancellationToken: ct
        );
    }

    // Invalidate cache when product is updated
    public async Task UpdateProductAsync(Product product, CancellationToken ct = default)
    {
        await _repository.UpdateAsync(product);
        await _cache.RemoveAsync($"product_{product.Id}", ct);
    }
}
