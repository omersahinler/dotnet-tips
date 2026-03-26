// Dapper — raw SQL, full control, ideal for complex reporting queries
// No change tracking overhead, no entity materialization for unused columns

using Dapper;
using System.Data;

public class OrderReportService
{
    private readonly IDbConnection _db;

    public OrderReportService(IDbConnection db)
    {
        _db = db;
    }

    // Complex reporting query — Dapper shines here
    // EF Core would struggle to generate this SQL cleanly
    public async Task<IEnumerable<OrderSummaryDto>> GetMonthlySummaryAsync(
        int year, int month)
    {
        const string sql = """
            SELECT 
                c.Name AS CustomerName,
                COUNT(o.Id) AS OrderCount,
                SUM(o.Total) AS TotalRevenue,
                AVG(o.Total) AS AverageOrderValue,
                MAX(o.CreatedAt) AS LastOrderDate
            FROM Orders o
            INNER JOIN Customers c ON o.CustomerId = c.Id
            WHERE 
                YEAR(o.CreatedAt) = @Year AND 
                MONTH(o.CreatedAt) = @Month AND
                o.Status = 'Completed'
            GROUP BY c.Id, c.Name
            ORDER BY TotalRevenue DESC
            """;

        return await _db.QueryAsync<OrderSummaryDto>(sql, new { Year = year, Month = month });
    }

    // Multi-join query — clean SQL, no EF Core navigation property chains
    public async Task<IEnumerable<ProductPerformanceDto>> GetProductPerformanceAsync(
        DateTime from, DateTime to)
    {
        const string sql = """
            SELECT 
                p.Name AS ProductName,
                p.Category,
                COUNT(oi.Id) AS TimesSold,
                SUM(oi.Quantity) AS TotalQuantitySold,
                SUM(oi.Price * oi.Quantity) AS TotalRevenue
            FROM OrderItems oi
            INNER JOIN Products p ON oi.ProductId = p.Id
            INNER JOIN Orders o ON oi.OrderId = o.Id
            WHERE o.CreatedAt BETWEEN @From AND @To
            GROUP BY p.Id, p.Name, p.Category
            ORDER BY TotalRevenue DESC
            """;

        return await _db.QueryAsync<ProductPerformanceDto>(sql, new { From = from, To = to });
    }
}

public record OrderSummaryDto(
    string CustomerName,
    int OrderCount,
    decimal TotalRevenue,
    decimal AverageOrderValue,
    DateTime LastOrderDate
);

public record ProductPerformanceDto(
    string ProductName,
    string Category,
    int TimesSold,
    int TotalQuantitySold,
    decimal TotalRevenue
);
