# back-to-reality-shared-extensions
General-purpose toolkit of utilities and extensions for everyday .NET code.

This repository contains the `Rumrunner0.BackToReality.SharedExtensions` class library and `Rumrunner0.BackToReality.SharedExtensions.Tests` test project. All the content in the repository is an original work created as a personal project to keep reusable everyday utilities in one place.

[![License](https://img.shields.io/github/license/rumrunner0/back-to-reality-shared-extensions?label=license)](https://github.com/rumrunner0/back-to-reality-shared-extensions/blob/main/LICENSE)
[![Nuget](https://img.shields.io/nuget/v/Rumrunner0.BackToReality.SharedExtensions?logo=nuget&label=nuget)](https://www.nuget.org/packages/Rumrunner0.BackToReality.SharedExtensions)

## Description
The `Rumrunner0.BackToReality.SharedExtensions` is a general-purpose class library of utilities and extension methods for everyday .NET code. It covers collections, cryptography, disposal, environment variables, exception guards and inspection, strings and object shaping, JSON serialization defaults, task awaiting, time truncation, and value objects.

The `Rumrunner0.BackToReality.SharedExtensions.Tests` is an xunit test project covering the behavior of the library.

## Installation
To install the package, use the following command:
```shell
$ dotnet add package Rumrunner0.BackToReality.SharedExtensions
```

## Usage
All types live under the `Rumrunner0.BackToReality.SharedExtensions` namespace. Each feature area below is a sub-namespace, for example `Rumrunner0.BackToReality.SharedExtensions.Collections`.

### Collections
Counting predicates for `IEnumerable<T>` that stop enumerating as soon as the answer is known:

- `None()`, `Some()`, `Many()` for the common cases.
- `Exactly(n)`, `AtLeast(n)`, `MoreThan(n)`, `LessThan(n)` for explicit bounds.
- `IsNullOrEmpty()` and `IsNotNullAndNotEmpty()`, annotated so the compiler narrows nullability after the check.

```csharp
using Rumrunner0.BackToReality.SharedExtensions.Collections;

var ready = requests.AtLeast(2) && requests.LessThan(100); // Never enumerates more than needed.
```

Helpers for lists and other collections:

- `Add(items)` overloads that accept sequences, which also allows mixing single items and sequences in a collection initializer.
- `AddNonNulls(items)` adds only the non-null values; `ArrayExtensions.CreateFromNonNulls(items)` builds an array the same way.
- `AddAndReturn(item)` and `AddAndReturnCollection(item)` add an item and return the item or the collection.
- `RemoveFirst()`, `RemoveLast()`, `RemoveFromStart(end)`, `RemoveToEnd(start)`, `RemoveBetween(start, end)` remove list ranges by inclusive indexes and return the list for chaining.
- `First()` and `Last()` for `IReadOnlyList<T>` without LINQ.
- `Deconstruct2()` through `Deconstruct8()` unpack a list into a value tuple.
- `StringJoin(separator)` joins items into a string (the default separator is a space).
- `Cycle(ct)` loops over a collection endlessly until the token is canceled or the collection becomes empty.
- `HashSetFactory.ReferenceEquality<T>()` creates a `HashSet<T>` that compares items by reference.

```csharp
IEnumerable<string> lines = ReadLines();
var merged = new List<string> { "header", lines, "footer" }; // Add(items) in an initializer.

var (host, port) = "localhost:8080".Split(':').Deconstruct2();
```

### Cryptography
AES-256-GCM encryption of strings. The nonce, the authentication tag, and the ciphertext are packed into a single Base64 blob (`nonce|tag|cipher`), so one string carries everything needed for decryption. UTF-8 handling is strict (ill-formed input throws instead of being silently replaced), and transient copies of the key and the plaintext are zeroed after use.

```csharp
using Rumrunner0.BackToReality.SharedExtensions.Cryptography;

var key = AesGcmSymmetricEncryption.GenerateKey();           // Base64-encoded 256-bit key.
var blob = AesGcmSymmetricEncryption.Encrypt("secret", key); // Base64 blob: nonce|tag|cipher.
var text = AesGcmSymmetricEncryption.Decrypt(blob, key);     // "secret".
```

`CryptographicOperationExtensions.FixedTimeEquals(left, right)` compares two strings without exiting early on the first mismatch, for values like tokens or password hashes.

### Disposing
`DisposableGroup` treats several disposables as one. Disposal runs in reverse order to mirror nested `using` blocks, happens once even with concurrent calls, and never stops halfway: exceptions from individual items are collected into `DisposalExceptions` instead of being thrown. The group also implements `IReadOnlyList<IDisposable>`, so the items stay accessible until disposal.

```csharp
using Rumrunner0.BackToReality.SharedExtensions.Disposing;

using var resources = new DisposableGroup([connection, transaction, reader]);
```

### Environment
Readers for environment variables that must be present. A missing or empty variable produces an `InvalidOperationException` that names the variable; numbers are parsed with the invariant culture.

```csharp
using Rumrunner0.BackToReality.SharedExtensions.Environment;

var connectionString = EnvironmentVariableExtensions.GetRequired("DB_CONNECTION_STRING");
var timeoutSeconds = EnvironmentVariableExtensions.GetRequiredInt("TIMEOUT_SECONDS");
```

### Exceptions
Guard clauses that capture the argument expression automatically and carry nullability attributes, so a failing check reports the right parameter name and a passing one removes the null warnings that follow:

- `ThrowIfNull(value)` for classes and `ThrowIfNullValue(value)` for nullable structs.
- `ThrowIfNullOrEmpty(value)` and `ThrowIfEmpty(value)` for strings and collections.
- `ThrowIfNullOrEmptyOrWhiteSpace(value)` and `ThrowIfEmptyOrWhiteSpace(value)` for strings.
- `ThrowIfAnyNull(collection)` for collections that must not contain `null` items.
- `Throw(message, argumentName)` to fail unconditionally.

```csharp
using Rumrunner0.BackToReality.SharedExtensions.Exceptions;

void Send(Message? message)
{
	ArgumentExceptionExtensions.ThrowIfNull(message); // ArgumentNullException with ParamName "message".
	Deliver(message.Body);                            // No null warning: the guard is annotated with [NotNull].
}
```

Helpers for inspecting caught exceptions, aware of every branch of an `AggregateException`:

- `IsOrHasInner<T>()` returns the first `T` found, starting from the exception itself.
- `HasInner<T>()` does the same but searches only the inner exceptions.
- `JoinMessages(separator)` flattens the messages of the whole chain into one string.

```csharp
try
{
	await ProcessAsync();
}
catch (Exception e) when (e.IsOrHasInner<TimeoutException>() is not null)
{
	logger.LogWarning("Timed out: {Messages}", e.JoinMessages("; "));
}
```

### Extensions
String helpers:

- Emptiness checks that narrow nullability: `IsNullOrEmpty()`, `IsEmpty()`, `IsNullOrEmptyOrWhitespace()`, `IsEmptyOrWhitespace()`.
- `IsValidJson()` for a quick syntax check.
- `SplitByWhitespace()` splits on any whitespace and drops empty entries.
- `ToGuid()` and `ToGuidOrNull()` parse a string into a `Guid`.
- `TryGetBytesFromBase64String(value, out bytes)` decodes Base64 without throwing.
- `Format(values)` as an extension form of `string.Format`.

Object shaping, for keeping a transformation in a single expression:

- `Shape(x => ...)` maps an object into another one.
- `Chain(x => ...)` runs an action (sync or async) and returns the object.
- `Follow(target)` switches the chain to another object.

```csharp
using Rumrunner0.BackToReality.SharedExtensions.Extensions;

var summary = order
	.Chain(o => logger.LogInformation("Processing {Id}", o.Id))
	.Shape(o => new OrderSummary(o.Id, o.Total));
```

`EmailAddressExtensions.PragmaticRegex()` is a source-generated, case-insensitive email pattern meant for practical validation rather than full RFC coverage.

### Serialization
`JsonSerializerOptionsExtensions.BetterWeb` is a shared, read-only `JsonSerializerOptions` instance: camelCase names, case-insensitive reading, strict number handling, indented output with tabs, `\n` line endings, no trailing commas. `ConfigureBetterWeb()` applies the same settings to an instance you own.

```csharp
using Rumrunner0.BackToReality.SharedExtensions.Serialization;

var json = JsonSerializer.Serialize(order, JsonSerializerOptionsExtensions.BetterWeb);
```

### Tasks
`ContinueWithoutContextCapture()` is `ConfigureAwait(false)` under a name that states its effect: the continuation does not marshal back to the captured context. Overloads cover `Task`, `Task<T>`, `ValueTask`, and `ValueTask<T>`.

```csharp
using Rumrunner0.BackToReality.SharedExtensions.Tasks;

var payload = await LoadAsync().ContinueWithoutContextCapture();
```

### Time
- `Truncate(resolution)` cuts a `DateTimeOffset` down to the nearest lower multiple of a tick resolution and keeps the offset.
- `TruncatedTimeProvider` is a `TimeProvider` whose `GetUtcNow()` is truncated to whole microseconds, which keeps timestamps stable when they round-trip through storage with microsecond precision.

```csharp
using Rumrunner0.BackToReality.SharedExtensions.Time;

var rounded = DateTimeOffset.UtcNow.Truncate(TimeSpan.TicksPerSecond);
```

### Value objects
`IValueObject<TSelf, TValue>` is a minimal contract for strongly typed wrappers over a single value: a `Value` property, equality, and a static `From` factory.

```csharp
using Rumrunner0.BackToReality.SharedExtensions.ValueObjects;

public readonly record struct UserId(Guid Value) : IValueObject<UserId, Guid>
{
	public static UserId From(Guid value) => new (value);
}
```

## Contributing
If you have any suggestions, ideas, or feedback to enhance the project, please feel free to create an issue. Your collaboration is welcomed to make this project a bit better.