# StringBuilder vs String Concatenation — C# Performance

## Scenario

A service that builds dynamic SQL queries, email templates, 
or report content by joining strings in a loop.
Getting this wrong creates serious memory pressure under load.

## The Difference

- String concatenation → creates a new string object on every operation
- StringBuilder → uses a mutable buffer, allocates once

## Files

- [BadExample.cs](BadExample.cs) → String concatenation in a loop
- [GoodExample.cs](GoodExample.cs) → StringBuilder equivalent

---

## Senaryo

Döngü içinde dinamik SQL sorgusu, e-posta şablonu veya 
rapor içeriği oluşturan bir servis.
Yanlış kullanılırsa yük altında ciddi memory baskısı yaratır.

## Fark

- String birleştirme → her işlemde yeni bir string nesnesi oluşturur
- StringBuilder → değiştirilebilir bir buffer kullanır, tek seferinde allocate eder
