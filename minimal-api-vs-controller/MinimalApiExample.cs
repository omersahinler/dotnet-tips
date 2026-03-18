// Minimal API — .NET 8+
// Less boilerplate, ~10% faster startup, scales well with endpoint grouping.

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products")
            .RequireAuthorization();

        group.MapGet("/{id}", async (int id, IProductService svc) =>
        {
            var product = await svc.GetAsync(id);
            return product is null ? Results.NotFound() : Results.Ok(product);
        });

        group.MapGet("/", async (string? category, IProductService svc) =>
        {
            var products = await svc.GetAllAsync(category);
            return Results.Ok(products);
        });

        group.MapPost("/", async (CreateProductRequest request, IProductService svc) =>
        {
            var product = await svc.CreateAsync(request);
            return Results.Created($"/api/products/{product.Id}", product);
        });

        group.MapDelete("/{id}", async (int id, IProductService svc) =>
        {
            await svc.DeleteAsync(id);
            return Results.NoContent();
        });

        return app;
    }
}

// Program.cs
// app.MapProductEndpoints();

public interface IProductService
{
    Task<Product?> GetAsync(int id);
    Task<List<Product>> GetAllAsync(string? category);
    Task<Product> CreateAsync(CreateProductRequest request);
    Task DeleteAsync(int id);
}

public record CreateProductRequest(string Name, decimal Price, string Category);
