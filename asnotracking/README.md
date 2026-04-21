# AsNoTracking — EF Core
# AsNoTracking — EF Core

---

## EN

EF Core tracks every entity you query by default.

This is useful for write operations — 
it lets you detect and save changes automatically.

But for read-only queries, tracking is pure overhead.
Every tracked entity holds a reference in the DbContext.
Under high load, this adds up fast.

### The rule

Read-only query → always AsNoTracking()
Write operation → tracking needed, skip AsNoTracking()

### Performance impact

AsNoTracking() gives you:
- Lower memory usage
- Faster query execution
- Less GC pressure

On high-traffic endpoints, the difference is significant.

### How to apply globally

If most of your queries are read-only, set it globally:

```csharp
optionsBuilder.UseQueryTrackingBehavior(
    QueryTrackingBehavior.NoTracking);
```

Then add tracking back only where needed:

```csharp
_db.Products.AsTracking().Where(...);
```

---

## TR

EF Core varsayılan olarak sorguladığın 
her entity'yi takip eder.

Bu write operasyonları için kullanışlı —
değişiklikleri otomatik algılayıp kaydeder.

Ama read-only sorgularda tracking sadece overhead.
Her takip edilen entity DbContext'te referans tutar.
Yüksek yük altında bu hızla birikeir.

### Kural

Read-only sorgu → her zaman AsNoTracking()
Write operasyonu → tracking gerekli, AsNoTracking() ekleme

### Performans etkisi

AsNoTracking() sağlar:
- Daha düşük memory kullanımı
- Daha hızlı sorgu yürütme
- Daha az GC baskısı

Yüksek trafikli endpoint'lerde fark belirgin.

### Global olarak uygulama

Sorgularının çoğu read-only ise global ayarla:

```csharp
optionsBuilder.UseQueryTrackingBehavior(
    QueryTrackingBehavior.NoTracking);
```

Sonra sadece gerektiğinde tracking ekle:

```csharp
_db.Products.AsTracking().Where(...);
```

---

## Code / Kod

See `AsNoTrackingExample.cs`

## Related / İlgili

- [IEnumerable vs IQueryable](../ienumerable-vs-iqueryable/)
- [Dapper vs EF Core](../dapper-vs-efcore/)
