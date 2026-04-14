// ❌ The bug — Transient IDisposable inside Singleton
public class ReportGenerator : IDisposable // Transient
{
    private readonly SqlConnection _connection;

    public ReportGenerator(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
    }

    public void Dispose()
    {
        _connection?.Dispose(); // Never called when inside Singleton
    }
}

public class ReportService // Singleton
{
    private readonly ReportGenerator _generator; // Transient — WRONG

    public ReportService(ReportGenerator generator)
    {
        _generator = generator; // captured — never disposed
    }
}

// Program.cs
builder.Services.AddSingleton<ReportService>();
builder.Services.AddTransient<ReportGenerator>(); // leaks here

// ✅ Fix Option 1 — Change to Scoped
builder.Services.AddScoped<ReportGenerator>();

// ✅ Fix Option 2 — IServiceScopeFactory
public class ReportService // Singleton
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ReportService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task GenerateAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var generator = scope.ServiceProvider
            .GetRequiredService<ReportGenerator>();

        await generator.GenerateAsync();
        // generator.Dispose() called automatically here
    }
}

// ✅ Enable scope validation — catches mismatches at startup
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});
