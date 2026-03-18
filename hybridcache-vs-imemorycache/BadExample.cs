// BAD: IMemoryCache — Cache Stampede Problem
// When cache expires, all concurrent requests hit the database simultaneously.
// In a 500K daily traffic system, this can cause serious DB overload.

using Microsoft.Extensions.Caching.Memory;

public class ProductService
{
    private readonly IMemoryCache _cache;
    private readonly IProductRepository _repository;
    private const string CacheKey = "product_{0}";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public ProductService(IMemoryCache cache, IProductRepository repository)
    {
        _cache = cache;
        _repository = repository;
    }

    public async Task<Product?> GetProductAsync(int productId)
    {
        var key = string.Format(CacheKey, productId);

        if (_cache.TryGetValue(key, out Product? cached))
            return cached;

        // PROBLEM: If cache is empty and 100 requests arrive at the same time,
        // all 100 will reach here and hit the database.
        // This is called a "cache stampede" or "thundering herd".
        var product = await _repository.GetByIdAsync(productId);

        _cache.Set(key, product, CacheDuration);

        return product;
    }
}

public record Product(int Id, string Name, decimal Price);

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
}
