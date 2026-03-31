# async/await vs Task.Run — C# Async

## Scenario

Two different situations in a backend service:
- Fetching data from a database or external API (I/O-bound)
- Generating a PDF report from thousands of orders (CPU-bound)

They look similar. They solve different problems.

## The Difference

- **async/await** → I/O-bound work. Thread is released while waiting, no extra thread consumed.
- **Task.Run** → CPU-bound work. Offloads heavy computation to thread pool.

## The Common Mistake

Wrapping I/O-bound work in Task.Run wastes a thread pool thread for no reason.
async/await already handles I/O without blocking any thread.

## Files

- [Examples.cs](Examples.cs) → Both approaches with real scenarios

---

## Senaryo

Bir backend servisinde iki farklı durum:
- Veritabanından veya harici API'den veri çekme (I/O-bound)
- Binlerce siparişten PDF raporu oluşturma (CPU-bound)

Benzer görünüyorlar. Farklı sorunları çözüyorlar.

## Fark

- **async/await** → I/O-bound iş. Thread beklerken serbest kalır, ekstra thread tüketmez.
- **Task.Run** → CPU-bound iş. Ağır hesaplamayı thread pool'a aktarır.

- 
