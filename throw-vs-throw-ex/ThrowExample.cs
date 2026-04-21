// ❌ throw ex — resets stack trace
public async Task ProcessAsync()
{
    try
    {
        await _service.DoWorkAsync();
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Processing failed");
        throw ex; // Stack trace now starts HERE — original lost
    }
}

// ✅ throw — preserves original stack trace
public async Task ProcessAsync()
{
    try
    {
        await _service.DoWorkAsync();
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Processing failed");
        throw; // Original stack trace preserved
    }
}

// ✅ Wrapping with context — InnerException preserved
public async Task ProcessOrderAsync(Order order)
{
    try
    {
        await _paymentService.ChargeAsync(order);
    }
    catch (Exception ex)
    {
        throw new OrderProcessingException(
            $"Failed to process order {order.Id}", ex);
        // ex is preserved as InnerException
    }
}

// ✅ When to catch and NOT re-throw
public async Task<bool> TryProcessAsync()
{
    try
    {
        await _service.DoWorkAsync();
        return true;
    }
    catch (KnownException ex)
    {
        _logger.LogWarning(ex, "Known failure, handled");
        return false; // handled — no re-throw needed
    }
}
