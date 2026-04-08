// Program.cs — Service registration
builder.Services.AddSingleton<IEmailService, EmailService>();   // stateless, thread-safe
builder.Services.AddScoped<IOrderService, OrderService>();      // per request
builder.Services.AddScoped<AppDbContext>();                     // always Scoped
builder.Services.AddTransient<INotificationSender, NotificationSender>(); // lightweight

// ❌ Common mistake #1 — DbContext as Singleton
builder.Services.AddSingleton<AppDbContext>(); // WRONG
// → Not thread-safe
// → Stale data across requests
// → Crashes under concurrent load

// ❌ Common mistake #2 — Scoped injected into Singleton
public class EmailService // Singleton
{
    private readonly IOrderService _orderService; // Scoped — WRONG

    public EmailService(IOrderService orderService)
    {
        _orderService = orderService; // captured forever
    }
}

// ✅ Correct — resolve Scoped inside Singleton via IServiceScopeFactory
public class EmailService // Singleton
{
    private readonly IServiceScopeFactory _scopeFactory;

    public EmailService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task SendAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var orderService = scope.ServiceProvider
            .GetRequiredService<IOrderService>();

        await orderService.ProcessAsync();
    }
}

// ❌ Common mistake #3 — Transient IDisposable in Singleton
public class ReportService // Singleton
{
    private readonly IDataReader _reader; // Transient + IDisposable — WRONG
    // Container won't dispose _reader — memory leak
}

// ✅ Correct — use IServiceScopeFactory here too
