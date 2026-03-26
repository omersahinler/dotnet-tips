// EF Core — LINQ, change tracking, migrations, domain operations
// Ideal for standard CRUD, domain logic, and entity lifecycle management

using Microsoft.EntityFrameworkCore;

public class OrderService
{
    private readonly AppDbContext _db;

    public OrderService(AppDbContext db)
    {
        _db = db;
    }

    // Standard CRUD — EF Core handles this cleanly
    public async Task<Order?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, ct);
    }

    // Change tracking — EF Core detects what changed, generates minimal UPDATE
    public async Task UpdateStatusAsync(int orderId, OrderStatus status, CancellationToken ct = default)
    {
        var order = await _db.Orders.FindAsync(new object[] { orderId }, ct);

        if (order is null)
            throw new NotFoundException($"Order {orderId} not found");

        order.UpdateStatus(status);

        // EF Core generates: UPDATE Orders SET Status = @status WHERE Id = @id
        // Only the changed column, not the entire entity
        await _db.SaveChangesAsync(ct);
    }

    // Bulk operation — EF Core 7+ ExecuteUpdateAsync
    // No entity loading, direct SQL, bypasses change tracker
    public async Task DeactivateExpiredOrdersAsync(CancellationToken ct = default)
    {
        await _db.Orders
            .Where(o => o.ExpiresAt < DateTime.UtcNow && o.Status == OrderStatus.Pending)
            .ExecuteUpdateAsync(s => s.SetProperty(o => o.Status, OrderStatus.Expired), ct);
    }

    // Migrations handle schema changes automatically
    // dotnet ef migrations add AddOrderExpiryDate
    // dotnet ef database update
}

public class Order
{
    public int Id { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime? ExpiresAt { get; private set; }
    public List<OrderItem> Items { get; private set; } = new();

    public void UpdateStatus(OrderStatus status)
    {
        // Domain logic lives here, not in the service
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Cannot update a completed order");

        Status = status;
    }
}

public enum OrderStatus { Pending, Completed, Expired }
public class OrderItem { public int Id { get; set; } }
public class NotFoundException(string message) : Exception(message);
