# IEnumerable vs IQueryable — EF Core

## Scenario

A product listing page with filtering, sorting and pagination.
Getting this wrong loads the entire table into memory on every request.

## The Difference

- IEnumerable → filter runs in C#, after fetching all rows from DB
- IQueryable → filter runs in SQL, only matching rows are fetched

## Files

- [BadExample.cs](BadExample.cs) → IEnumerable loading entire table into memory
- [GoodExample.cs](GoodExample.cs) → IQueryable filtering at the database level

---

## Senaryo

Filtreleme, sıralama ve sayfalama olan bir ürün listeleme sayfası.
Yanlış kullanılırsa her istekte tüm tablo belleğe yüklenir.

## Fark

- IEnumerable → filtre C#'ta çalışır, tüm satırlar DB'den çekilir
- IQueryable → filtre SQL'de çalışır, sadece eşleşen satırlar gelir
