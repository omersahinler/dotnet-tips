// class — mutable, has behavior, changes over time
// Use for objects with lifecycle: services, entities, managers
public class ShoppingCart
{
    private readonly List<CartItem> _items = new();

    public IReadOnlyList<CartItem> Items => _items;

    public void Add(CartItem item) => _items.Add(item);

    public void Remove(int productId) =>
        _items.RemoveAll(i => i.ProductId == productId);

    public decimal Total() => _items.Sum(i => i.Price * i.Quantity);
}

public class CartItem
{
    public int ProductId { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

// record — immutable, value equality, just data
// Use for DTOs, API responses, domain events
// Two records with same values are considered equal
public record OrderCreatedEvent(
    int OrderId,
    string CustomerId,
    decimal Total,
    DateTime CreatedAt
);

// Value equality in action:
// var e1 = new OrderCreatedEvent(1, "c1", 100, date);
// var e2 = new OrderCreatedEvent(1, "c1", 100, date);
// e1 == e2 → true (class would return false)

// struct — value type, stack allocated, no GC pressure
// Use for small, high-frequency data
// 500K daily transactions — struct avoids heap allocation entirely
public readonly struct Money
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Currency mismatch");

        return new Money(Amount + other.Amount, Currency);
    }

    public override string ToString() => $"{Amount} {Currency}";
}

// Practical rule:
// Behavior + lifecycle    → class
// Immutable data transfer → record
// Small + high frequency  → struct
