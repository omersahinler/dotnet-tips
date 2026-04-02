using Microsoft.Extensions.Caching.Memory;

public class ProductService
{
    private readonly AppDbContext _db;
    private readonly IMemoryCache _cache;

    public ProductService(AppDbContext db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
    }

    // Task — always allocates, even on cache hit
    public async Task<Product?> GetWithTaskAsync(int id)
    {
        if (_cache.TryGetValue($"product:{id}", out Product? cached))
            return cached; // Task object still created here

        var product = await _db.Products.FindAsync(id);

        if (product is not null)
            _cache.Set($"product:{id}", product, TimeSpan.FromMinutes(5));

        return product;
    }

    // ValueTask — zero allocation on cache hit (sync path)
    public async ValueTask<Product?> GetWithValueTaskAsync(int id)
    {
        if (_cache.TryGetValue($"product:{id}", out Product? cached))
            return cached; // No allocation — returns synchronously

        var product = await _db.Products.FindAsync(id);

        if (product is not null)
            _cache.Set($"product:{id}", product, TimeSpan.FromMinutes(5));

        return product;
    }
}

// ⚠️ Common mistake — never await ValueTask twice
var vt = service.GetWithValueTaskAsync(1);
var result = await vt;  // OK
var again  = await vt;  // UNDEFINED BEHAVIOR — don't do this

// ✅ If you need to await multiple times, convert to Task first
var vt2 = service.GetWithValueTaskAsync(1);
var task = vt2.AsTask();
var r1 = await task;
var r2 = await task; // Safe
