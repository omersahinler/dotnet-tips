// BAD: String concatenation in a loop
// Each += creates a new string object in memory.
// With 1000 iterations, this creates 1000 intermediate strings.
// Garbage collector has to clean all of them up.

public class ReportGenerator
{
    public string BuildReport(IEnumerable<Order> orders)
    {
        string report = "";

        foreach (var order in orders)
        {
            // Every += allocates a new string and copies the previous content
            // With 10K orders, this is 10K allocations + increasing copy cost
            report += $"Order #{order.Id} | {order.CustomerName} | {order.Total:C}\n";
        }

        return report;
    }

    public string BuildDynamicQuery(List<int> productIds)
    {
        string query = "SELECT * FROM Products WHERE Id IN (";

        for (int i = 0; i < productIds.Count; i++)
        {
            query += productIds[i];

            if (i < productIds.Count - 1)
                query += ", ";
        }

        query += ")";

        // SQL: SELECT * FROM Products WHERE Id IN (1, 2, 3, ...)
        // Problem: each iteration allocates a new string
        return query;
    }
}

public record Order(int Id, string CustomerName, decimal Total);
