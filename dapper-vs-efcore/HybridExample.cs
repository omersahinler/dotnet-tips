// Hybrid approach — EF Core for domain operations, Dapper for reporting
// Same database, same connection string, two tools for two different jobs
// This is the most common pattern in high-traffic production systems

using Dapper;
using Microsoft.EntityFrameworkCore;

public class OrderHybridService
{
    private readonly AppDbContext _db;

    public OrderHybridService(AppDbContext db)
    {
        _db = db;
    }

    // EF Core — domain operation with business logic
    public async Task<Order> CreateOrderAsync(
        CreateOrderRequest request, CancellationToken ct = default)
    {
        var order = new Order
        {
            CustomerId = request.CustomerId,
            Items = request.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync(ct);
        return order;
    }

    // Dapper — complex reporting query on the same database
    // EF Core connection is reused — no second connection pool
    public async Task<IEnumerable<CustomerOrderSummary>> GetCustomerSummaryAsync(
        int customerId)
    {
        // Get the underlying connection from EF Core's DbContext
        var connection = _db.Database.GetDbConnection();

        const string sql = """
            SELECT 
                YEAR(o.CreatedAt) AS Year,
                MONTH(o.CreatedAt) AS Month,
                COUNT(o.Id) AS OrderCount,
                SUM(o.Total) AS TotalSpent
            FROM Orders o
            WHERE o.CustomerId = @CustomerId
            GROUP BY YEAR(o.CreatedAt), MONTH(o.CreatedAt)
            ORDER BY Year DESC, Month DESC
            """;

        return await connection.QueryAsync<CustomerOrderSummary>(
            sql, new { CustomerId = customerId });
    }

    // Rule:
    // Domain operations (create, update, delete) → EF Core
    // Reporting, analytics, complex joins → Dapper
    // Both use the same DbConnection — no extra overhead
}

public record CreateOrderRequest(
    int CustomerId,
    List<OrderItemRequest> Items
);

public record OrderItemRequest(int ProductId, int Quantity, decimal Price);

public record CustomerOrderSummary(
    int Year,
    int Month,
    int OrderCount,
    decimal TotalSpent
);
