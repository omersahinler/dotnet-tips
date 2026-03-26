# Dapper vs EF Core

## Scenario

A high-traffic backend with two different needs:
- Complex reporting queries with custom joins and aggregations
- Standard CRUD operations for domain entities

One tool doesn't fit both. That's where using them together makes sense.

## The Difference

- **Dapper** → raw SQL, lightweight, full control, faster for complex queries
- **EF Core** → LINQ, migrations, change tracking, better for domain operations

## When to use which

| Situation | Tool |
|-----------|------|
| Complex reporting, custom joins | Dapper |
| Standard CRUD, domain logic | EF Core |
| Bulk operations | EF Core (ExecuteUpdateAsync) |
| Performance-critical read queries | Dapper |

## Files

- [DapperExample.cs](DapperExample.cs) → Raw SQL queries with Dapper
- [EFCoreExample.cs](EFCoreExample.cs) → LINQ queries with EF Core
- [HybridExample.cs](HybridExample.cs) → Using both in the same service

---

## Senaryo

Yüksek trafikli bir backend'de iki farklı ihtiyaç:
- Özel join'ler ve aggregation'larla karmaşık raporlama sorguları
- Domain entity'leri için standart CRUD operasyonları

Tek araç ikisine de uymaz. İkisini birlikte kullanmak burada anlam kazanır.
