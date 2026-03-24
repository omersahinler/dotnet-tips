# record vs class vs struct — C# 9+

## Scenario

Three common situations in a high-traffic e-commerce backend:
- A shopping cart that changes state over time
- An order event that gets passed between services
- A money value created 500K times per day

Each one needs a different type.

## The Difference

- **class** → reference type, mutable, has behavior and lifecycle
- **record** → reference type, immutable by default, value equality built-in
- **struct** → value type, lives on the stack, no heap allocation

## When to use which

| Type | Use when |
|------|----------|
| class | Object has behavior, changes over time |
| record | Data is immutable, used as DTO or event |
| struct | Small, high-frequency, performance critical |

## Files

- [Examples.cs](Examples.cs) → All three types with real scenarios

---

## Senaryo

Yüksek trafikli bir e-ticaret backend'inde üç yaygın durum:
- Zamanla değişen bir alışveriş sepeti
- Servisler arasında taşınan bir sipariş eventi
- Günde 500K kez oluşturulan bir para birimi değeri

Her biri farklı bir tip gerektirir.

## Fark

- **class** → referans tipi, mutable, davranışı ve yaşam döngüsü var
- **record** → referans tipi, varsayılan olarak immutable, value equality built-in
- **struct** → değer tipi, stack'te yaşar, heap allocation yok

- 
