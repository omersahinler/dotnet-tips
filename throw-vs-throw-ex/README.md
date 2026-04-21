# throw vs throw ex — C#
# throw vs throw ex — C#

---

## EN

One of the smallest mistakes in C# with the 
biggest impact during debugging.

throw re-throws the original exception with 
the stack trace intact.

throw ex resets the stack trace to the current line.
You lose where the exception actually originated.

### When it matters most

In production, when something goes wrong and 
you need to trace the exact origin of an error.

With throw ex, your stack trace points to the 
catch block — not the real source.
You lose hours chasing the wrong location.

### The rule

Never use throw ex when re-throwing.
Use throw alone.

If you need to add context, wrap it:

```csharp
throw new OrderException("Failed to process", ex);
```

This preserves the original exception as InnerException.

---

## TR

C#'taki en küçük hatalardan biri ama 
debug sırasında en büyük etkiyi yaratır.

throw, orijinal exception'ı stack trace korunarak 
yeniden fırlatır.

throw ex, stack trace'i mevcut satıra sıfırlar.
Exception'ın gerçekte nerede oluştuğunu kaybedersin.

### En çok ne zaman önemli

Production'da bir şeyler ters gittiğinde ve 
hatanın tam kaynağını izlemen gerektiğinde.

throw ex ile stack trace catch bloğuna işaret eder,
gerçek kaynağa değil. Yanlış yeri takip ederek
saatler harcarsın.

### Kural

Re-throw ederken asla throw ex kullanma.
Sadece throw kullan.

Bağlam eklemen gerekiyorsa wrap et:

```csharp
throw new OrderException("Failed to process", ex);
```

Orijinal exception InnerException olarak korunur.

---

## Code / Kod

See `ThrowExample.cs`

## Related / İlgili

- [async/await vs Task.Run](../async-await-vs-taskrun/)
