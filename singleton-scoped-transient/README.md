# Singleton vs Scoped vs Transient — .NET DI Lifetimes
# Singleton vs Scoped vs Transient — .NET DI Yaşam Döngüleri

---

## EN

Three lifetimes. One wrong choice can cause memory leaks,
stale data, or crashes under load.

### Singleton
Created once. Lives for the app's lifetime.
Safe for stateless, thread-safe services.

### Scoped
Created once per HTTP request.
Disposed when the request ends.
Default choice for most services.

### Transient
New instance every time it's injected.
Cheap to create, short-lived.
Watch out with IDisposable — container won't dispose them in Singletons.

### The most common mistake

Registering DbContext as Singleton.
EF Core's DbContext is not thread-safe.
Under concurrent requests: stale data, exceptions, crashes.

Always register DbContext as Scoped.

### The second most common mistake

Injecting a Scoped service into a Singleton.
The Scoped service gets captured — it lives as long as the Singleton.
Effectively becomes a Singleton. Breaks expected behavior.

---

## TR

Üç yaşam döngüsü. Yanlış bir seçim memory leak,
stale data veya yük altında crash'e yol açabilir.

### Singleton
Bir kez oluşturulur. Uygulama kapanana kadar yaşar.
Stateless, thread-safe servisler için güvenli.

### Scoped
Her HTTP request için bir kez oluşturulur.
Request bitince dispose edilir.
Çoğu servis için varsayılan tercih.

### Transient
Her injection'da yeni instance.
Oluşturması ucuz, kısa ömürlü.
IDisposable ile dikkat — Singleton içinde container dispose etmez.

### En sık yapılan hata

DbContext'i Singleton olarak kaydetmek.
EF Core'un DbContext'i thread-safe değil.
Eş zamanlı isteklerde: stale data, exception, crash.

DbContext'i her zaman Scoped olarak kaydet.

### İkinci en sık hata

Scoped servisi Singleton'a inject etmek.
Scoped servis yakalanır — Singleton kadar yaşar.
Efektif olarak Singleton'a döner. Beklenen davranışı bozar.

---

## Code / Kod

See `DependencyInjectionExample.cs`

## Related / İlgili

- [ValueTask vs Task](../valuetask-vs-task/)
- [async/await vs Task.Run](../async-await-vs-taskrun/)
