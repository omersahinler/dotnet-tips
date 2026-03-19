// GOOD: StringBuilder — single buffer, no intermediate allocations
// Append operations modify the same buffer in memory.
// Significantly less pressure on the garbage collector under load.

public class ReportGeneratorOptimized
{
    public string BuildReport(IEnumerable<Order> orders)
    {
        var sb = new StringBuilder();

        foreach (var order in orders)
        {
            // Appends to the same buffer — no new string allocation
            sb.Append($"Order #{order.Id} | {order.CustomerName} | {order.Total:C}\n");
        }

        // Single string created only at the end
        return sb.ToString();
    }

    public string BuildDynamicQuery(List<int> productIds)
    {
        var sb = new StringBuilder("SELECT * FROM Products WHERE Id IN (");

        for (int i = 0; i < productIds.Count; i++)
        {
            sb.Append(productIds[i]);

            if (i < productIds.Count - 1)
                sb.Append(", ");
        }

        sb.Append(')');

        return sb.ToString();
    }

    // When to use string.Join instead — cleaner for simple cases
    public string BuildQueryWithJoin(List<int> productIds)
    {
        var ids = string.Join(", ", productIds);
        return $"SELECT * FROM Products WHERE Id IN ({ids})";
    }

    // When NOT to use StringBuilder
    // Simple one-time concatenation — compiler optimizes this already
    public string BuildSimpleMessage(string name, int orderId)
    {
        // This is fine — no loop, no repeated allocation
        return $"Hello {name}, your order #{orderId} is confirmed.";
    }
}
