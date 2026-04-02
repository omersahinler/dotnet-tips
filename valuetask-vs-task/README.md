# ValueTask vs Task — When Allocation Matters
# ValueTask vs Task — Allocation Önemli Olduğunda

---

## EN

Most .NET developers use Task everywhere. That's usually fine.

But in hot paths — endpoints called thousands of times per second —
every Task allocation adds GC pressure. Over time, that adds up.

### The Problem

Task always allocates a heap object, even when the result
is available synchronously (e.g. cache hit).

ValueTask skips the allocation entirely on the sync path.

### Real-World Example

Imagine a product endpoint called 50,000 times/day.
80% of requests are cache hits.

With Task: 50,000 heap allocations/day
With ValueTask: ~10,000 allocations/day (only cache misses)

### When to Use ValueTask

✅ Hot path that often completes synchronously
✅ Cache-first patterns
✅ High-frequency interface implementations

### When NOT to Use ValueTask

❌ Path is always async (Task is cheaper)
❌ You need to await it multiple times
❌ You need to store it in a field

### The Golden Rule

If in doubt, use Task.
Reach for ValueTask only when you can measure the difference.

---

## TR

Çoğu .NET developer her yerde Task kullanır. Genellikle sorun olmaz.

Ama saniyede binlerce kez çağrılan hot path'lerde —
her Task allocation GC baskısı yaratır. Zamanla bu birikir.

### Problem

Task, sonuç senkron olarak hazır olsa bile (örn. cache hit)
her zaman heap'te bir nesne oluşturur.

ValueTask, senkron path'te allocation'ı tamamen atlar.

### Gerçek Dünya Örneği

Günde 50.000 kez çağrılan bir product endpoint düşünün.
İsteklerin %80'i cache hit.

Task ile: Günde 50.000 heap allocation
ValueTask ile: Günde ~10.000 allocation (sadece cache miss'ler)

### ValueTask Ne Zaman Kullanılır

✅ Sık sık senkron tamamlanan hot path'ler
✅ Cache-first pattern'lar
✅ Yüksek frekanslı interface implementasyonları

### ValueTask Ne Zaman Kullanılmaz

❌ Path her zaman async ise (Task daha ucuz)
❌ Birden fazla await gerekiyorsa
❌ Field'da saklanacaksa

### Altın Kural

Şüphe duyduğunda Task kullan.
ValueTask'a ancak farkı ölçebildiğinde geç.

---

## Code / Kod

See `ValueTaskExample.cs`
