// ❌ Without AsNoTracking — every entity tracked
public async Task<List<Product>> GetActiveProductsAsync()
{
    return await _db.Products
        .Where(p => p.IsActive)
        .ToListAsync();
    // DbContext tracks all returned entities
    // Memory grows with every request under load
}

// ✅ With AsNoTracking — read-only, no overhead
public async Task<List<Product>> GetActiveProductsAsync()
{
    return await _db.Products
        .Where(p => p.IsActive)
        .AsNoTracking()
        .ToListAsync();
    // No change tracking
    // Faster, lower memory usage
}

// ✅ Global setting — NoTracking by default
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
           .UseQueryTrackingBehavior(
               QueryTrackingBehavior.NoTracking));

// ✅ Re-enable tracking only for write operations
public async Task UpdateProductAsync(int id, string name)
{
    var product = await _db.Products
        .AsTracking()
        .FirstOrDefaultAsync(p => p.Id == id);

    if (product is null) return;

    product.Name = name;
    await _db.SaveChangesAsync();
}

// ✅ AsNoTrackingWithIdentityResolution
// Use when query returns same entity multiple times
var orders = await _db.Orders
    .Include(o => o.Customer)
    .AsNoTrackingWithIdentityResolution()
    .ToListAsync();
// Prevents duplicate objects for same entity
// Better than AsNoTracking() with Include()
