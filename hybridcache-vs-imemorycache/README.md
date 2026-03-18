# HybridCache vs IMemoryCache

## Scenario

An e-commerce product detail page receiving 500K+ daily requests.
Going to the database on every request is not an option.

Classic solution: IMemoryCache. But it has a problem under high traffic.

## The Problem: Cache Stampede

When the cache expires and 100 requests arrive simultaneously:
- IMemoryCache → all 100 hit the database
- HybridCache → only 1 hits the database, the other 99 wait and share the same result

## Files

- [BadExample.cs](BadExample.cs) → Cache stampede problem with IMemoryCache
- [GoodExample.cs](GoodExample.cs) → Solution with HybridCache
- [WithRedis.cs](WithRedis.cs) → HybridCache + Redis (distributed cache)

## Setup
```bash
# HybridCache
dotnet add package Microsoft.Extensions.Caching.Hybrid

# For Redis support
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis
```

---

## Senaryo

Günde 500K+ istek alan bir e-ticaret ürün detay sayfası.
Her istek için veritabanına gitmek mümkün değil.

Klasik çözüm: IMemoryCache. Ama yüksek trafikte bir sorunu var.

## Sorun: Cache Stampede

Cache süresi dolduğunda 100 istek aynı anda gelirse:
- IMemoryCache → 100 istek de veritabanına gider
- HybridCache → 1 istek veritabanına gider, 99'u bekler ve aynı sonucu alır
