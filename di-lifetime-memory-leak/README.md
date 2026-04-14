# DI Lifetime Mismatch — Memory Leak
# DI Lifetime Uyumsuzluğu — Memory Leak

---

## EN

One of the most common silent killers in .NET applications.

No exception. No obvious error.
Just memory climbing slowly until the pod restarts.

### What happens

When a Transient service implementing IDisposable
is injected into a Singleton, the DI container
holds a reference to it forever.

The Transient service is never disposed.
It accumulates with every request.

### Symptoms

- Memory usage grows slowly over hours
- Pod/process restarts every few hours
- No errors in logs — just gradual degradation

### The fix

Option 1: Change the Transient to Scoped
Option 2: Use IServiceScopeFactory in the Singleton

### How to detect it

Enable scope validation in development:

builder.Services.AddDbContext<AppDbContext>(options => { },
    ServiceLifetime.Scoped);

// This throws at startup if lifetime mismatch detected:
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

---

## TR

.NET uygulamalarındaki en yaygın sessiz sorunlardan biri.

Exception yok. Belirgin hata yok.
Sadece pod restart edene kadar yavaşça yükselen memory.

### Ne oluyor

IDisposable implement eden bir Transient servis
Singleton'a inject edildiğinde, DI container
ona sonsuza kadar referans tutar.

Transient servis hiç dispose edilmez.
Her request'te birikir.

### Belirtiler

- Saatler içinde yavaş yükselen memory kullanımı
- Pod/process her birkaç saatte restart
- Loglarda hata yok — sadece yavaş bozulma

### Çözüm

Seçenek 1: Transient'i Scoped'a çevir
Seçenek 2: Singleton içinde IServiceScopeFactory kullan

### Nasıl tespit edilir

Development'ta scope validation aç —
lifetime uyumsuzluğu varsa startup'ta hata verir.

---

## Code / Kod

See `MemoryLeakExample.cs`

## Related / İlgili

- [Singleton vs Scoped vs Transient](../singleton-scoped-transient/)
