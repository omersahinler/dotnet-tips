// HybridCache + Redis — Distributed Cache Setup
// L1: In-memory cache (fast, per instance)
// L2: Redis (shared across all instances)
// When L1 expires, HybridCache checks L2 before hitting the database.

using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.StackExchangeRedis;

public static class CacheConfiguration
{
    public static IServiceCollection AddHybridCacheWithRedis(
        this IServiceCollection services,
        string redisConnectionString)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = "myapp:";
        });

        services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = 1024 * 1024; // 1MB

            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(10),
                LocalCacheExpiration = TimeSpan.FromMinutes(2)
            };
        });

        return services;
    }
}

// Usage in Program.cs:
// builder.Services.AddHybridCacheWithRedis("localhost:6379");
