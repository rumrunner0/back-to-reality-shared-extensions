# Back to reality: Shared extensions

General-purpose toolkit of utilities and extensions for everyday .NET code.

```
dotnet add package Rumrunner0.BackToReality.SharedExtensions
```

## What's inside

| Area            | Highlights                                                                                                                                                                                                                                                                                                                     |
|-----------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `Collections`   | Bounded counting predicates (`Some`, `None`, `Many`, `Exactly`, `AtLeast`, `MoreThan`, `LessThan`) that never enumerate further than needed; inclusive range removal for lists; tuple deconstruction of lists; collection-initializer `Add` for any `ICollection<T>`; reference-equality hash sets; `Cycle` over a collection. |
| `Cryptography`  | AES-256-GCM string encryption packed into a single Base64 blob (`nonce\|tag\|cipher`), with strict UTF-8 and zeroing of transient key material; fixed-time string equality.                                                                                                                                                    |
| `Disposing`     | `DisposableGroup` treats many disposables as one: reverse-order disposal, thread-safe single shot, failures collected instead of thrown.                                                                                                                                                                                       |
| `Environment`   | Required environment variables (`GetRequired`, `GetRequiredInt`) with culture-invariant parsing.                                                                                                                                                                                                                               |
| `Exceptions`    | Guard helpers (`ThrowIfNull`, `ThrowIfNullOrEmpty`, `ThrowIfAnyNull`, ...) that capture the argument expression; `AggregateException`-aware inner-exception search (`IsOrHasInner`, `HasInner`) and message joining (`JoinMessages`).                                                                                          |
| `Extensions`    | String emptiness checks, JSON validation, Base64 decoding, GUID parsing, whitespace splitting, and fluent object shaping (`Shape`, `Chain`, `Follow`).                                                                                                                                                                         |
| `Serialization` | `JsonSerializerOptionsExtensions.BetterWeb` — a shared, read-only `JsonSerializerOptions` preconfigured with sane web defaults.                                                                                                                                                                                                |
| `Tasks`         | `ContinueWithoutContextCapture` — `ConfigureAwait(false)` with a name that says what it does, for `Task`, `Task<T>`, `ValueTask`, and `ValueTask<T>`.                                                                                                                                                                          |
| `Time`          | `DateTimeOffset.Truncate(resolution)` and `TruncatedTimeProvider` which is a `TimeProvider` truncated to microseconds for storage-friendly timestamps.                                                                                                                                                                         |
| `ValueObjects`  | `IValueObject<TSelf, TValue>` — a minimal contract for strongly-typed value wrappers.                                                                                                                                                                                                                                          |

## Some examples

Counting without over-enumerating:

```csharp
using Rumrunner0.BackToReality.SharedExtensions.Collections;

var ready = requests.AtLeast(2) && requests.LessThan(100); // enumerates at most 100 items
```

Guards that report the caller's argument name and teach the compiler:

```csharp
using Rumrunner0.BackToReality.SharedExtensions.Exceptions;

void Send(Message? message)
{
	ArgumentExceptionExtensions.ThrowIfNull(message); // ArgumentNullException with ParamName "message"
	Deliver(message.Body); // no null warning — the guard is annotated with [NotNull]
}
```

Symmetric encryption in one line each:

```csharp
using Rumrunner0.BackToReality.SharedExtensions.Cryptography;

var key = AesGcmSymmetricEncryption.GenerateKey();           // Base64-encoded 256-bit key
var blob = AesGcmSymmetricEncryption.Encrypt("secret", key); // Base64: nonce|tag|cipher
var text = AesGcmSymmetricEncryption.Decrypt(blob, key);     // "secret"
```

Many disposables as one:

```csharp
using Rumrunner0.BackToReality.SharedExtensions.Disposing;

using var resources = new DisposableGroup([connection, transaction, reader]);
// Disposed in reverse order; failures are collected into resources.DisposalExceptions.
```