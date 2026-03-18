// GOOD: IQueryable — Filtering happens at the database level
// Builds the SQL query first, executes only when needed.
// Only matching rows are fetched from the database.

using Microsoft.EntityFrameworkCore;

public class ProductQueryServiceGood
{
    private readonly AppDbContext _db;

    public ProductQueryServiceGood(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Product>> GetExpensiveProductsAsync(decimal minPrice)
    {
        // IQueryable builds the query — nothing executed yet
        IQueryable<Product> query = _db.Products;

        // Each LINQ operator adds to the SQL query
        query = query.Where(p => p.Price > minPrice);

        // SQL: SELECT * FROM Products WHERE Price > @minPrice
        // Only matching rows are fetched
        return await query.ToListAsync();
    }

    public async Task<List<Product>> GetPagedProductsAsync(
        decimal minPrice,
        string? category,
        int page,
        int pageSize)
    {
        IQueryable<Product> query = _db.Products;

        // Conditions are added dynamically — still no DB call
        if (minPrice > 0)
            query = query.Where(p => p.Price > minPrice);

        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category);

        // SQL: SELECT * FROM Products
        //      WHERE Price > @minPrice AND Category = @category
        //      ORDER BY Price
        //      OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY
        return await query
            .OrderBy(p => p.Price)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    // Rule: expose IEnumerable from repositories, build with IQueryable internally
    public IEnumerable<Product> GetAll() =>
        _db.Products.AsNoTracking();
}

public class AppDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
}

public record Product(int Id, string Name, decimal Price, string Category);


