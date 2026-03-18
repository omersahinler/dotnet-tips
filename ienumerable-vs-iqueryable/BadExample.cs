// BAD: IEnumerable — Filtering happens in memory
// Fetches ALL rows from the database, then filters in C#.
// On a table with 100K products, this loads 100K rows on every request.

using Microsoft.EntityFrameworkCore;

public class ProductQueryServiceBad
{
    private readonly AppDbContext _db;

    public ProductQueryServiceBad(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Product>> GetExpensiveProductsAsync(decimal minPrice)
    {
        // ToListAsync() executes immediately — pulls ALL products into memory
        // SQL: SELECT * FROM Products
        IEnumerable<Product> products = await _db.Products.ToListAsync();

        // Filtering happens here in C#, after all rows are already loaded
        return products.Where(p => p.Price > minPrice).ToList();
    }

    public async Task<List<Product>> GetPagedProductsAsync(int page, int pageSize)
    {
        // Same problem with pagination
        // SQL: SELECT * FROM Products — entire table!
        IEnumerable<Product> products = await _db.Products.ToListAsync();

        return products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }
}
