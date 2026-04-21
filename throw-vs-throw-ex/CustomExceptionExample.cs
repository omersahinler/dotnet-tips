// Define your hierarchy once
public abstract class DomainException : Exception
{
    protected DomainException(string message) 
        : base(message) { }
    
    protected DomainException(string message, Exception inner) 
        : base(message, inner) { }
}

public class OrderNotFoundException : DomainException
{
    public int OrderId { get; }
    
    public OrderNotFoundException(int orderId) 
        : base($"Order {orderId} not found")
    {
        OrderId = orderId;
    }
}

public class PaymentFailedException : DomainException
{
    public PaymentFailedException(string reason) 
        : base($"Payment failed: {reason}") { }
    
    public PaymentFailedException(string reason, Exception inner) 
        : base($"Payment failed: {reason}", inner) { }
}

// Usage — throw at the source, no catch-rethrow
public async Task<Order> GetOrderAsync(int id)
{
    var order = await _db.Orders.FindAsync(id);
    
    if (order is null)
        throw new OrderNotFoundException(id); // clean stack trace
    
    return order;
}

// Global exception handler — catch once at the top
app.UseExceptionHandler(err => err.Run(async ctx =>
{
    var ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
    
    var response = ex switch
    {
        OrderNotFoundException e => 
            Results.NotFound(e.Message),
        PaymentFailedException e => 
            Results.BadRequest(e.Message),
        DomainException e => 
            Results.UnprocessableEntity(e.Message),
        _ => Results.Problem()
    };
    
    await response.ExecuteAsync(ctx);
}));
