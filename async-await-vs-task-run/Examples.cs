// async/await vs Task.Run — real scenarios
// They look similar but solve fundamentally different problems

using Microsoft.EntityFrameworkCore;

public class OrderService
{
    private readonly AppDbContext _db;
    private readonly HttpClient _httpClient;

    public OrderService(AppDbContext db, HttpClient httpClient)
    {
        _db = db;
        _httpClient = httpClient;
    }

    // CORRECT: async/await for I/O-bound work
    // Thread is released while waiting for DB response
    // No extra thread consumed — scales well under high load
    public async Task<Order?> GetOrderAsync(int id, CancellationToken ct = default)
    {
        return await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, ct);
    }

    // CORRECT: async/await for HTTP calls
    // Same principle — thread freed while waiting for response
    public async Task<PaymentResult> ProcessPaymentAsync(
        PaymentRequest request, CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "/api/payments", request, ct);

        return await response.Content.ReadFromJsonAsync<PaymentResult>(ct)
            ?? throw new InvalidOperationException("Empty payment response");
    }

    // CORRECT: Task.Run for CPU-bound work
    // Heavy computation — offload to thread pool to free request thread
    // In a 500K daily transaction system, blocking the request thread here
    // would exhaust the thread pool under load
    public async Task<byte[]> GenerateMonthlyReportAsync(
        int year, int month, CancellationToken ct = default)
    {
        var orders = await _db.Orders
            .Where(o => o.CreatedAt.Year == year && o.CreatedAt.Month == month)
            .ToListAsync(ct);

        // CPU-bound: complex PDF generation with charts and calculations
        return await Task.Run(() => BuildPdfReport(orders), ct);
    }

    // WRONG: Wrapping I/O in Task.Run
    // This wastes a thread pool thread for no reason
    // The DB call is already async — Task.Run adds overhead, not benefit
    public async Task<Order?> WrongApproachAsync(int id)
    {
        return await Task.Run(async () =>
            await _db.Orders.FindAsync(id)); // ❌ unnecessary thread consumption
    }

    // WRONG: Blocking async code synchronously
    // .Result or .Wait() can cause deadlocks in ASP.NET Core
    public Order? AnotherWrongApproach(int id)
    {
        return _db.Orders.FindAsync(id).Result; // ❌ potential deadlock
    }

    // Rule:
    // Waiting for something (DB, HTTP, file, queue) → async/await
    // Computing something (PDF, image, encryption, algorithm) → Task.Run

    private byte[] BuildPdfReport(List<Order> orders)
    {
        // Heavy CPU work: aggregations, chart generation, PDF rendering
        // This is intentionally synchronous — it's pure computation
        return Array.Empty<byte>(); // placeholder
    }
}

public record PaymentRequest(int OrderId, decimal Amount, string Currency);
public record PaymentResult(bool Success, string TransactionId);
public class Order
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}
public class OrderItem { public int Id { get; set; } }
