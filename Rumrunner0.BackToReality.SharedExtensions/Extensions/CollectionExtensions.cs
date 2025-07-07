using System.Collections.Generic;
using System.Threading;

namespace Rumrunner0.BackToReality.SharedExtensions.Extensions;

/// <summary>
/// Extensions for collection types.
/// </summary>
public static class CollectionExtensions
{
	/// <summary>
	/// Determines whether a collection is <c>null</c> or empty.
	/// </summary>
	/// <param name="source">The collection.</param>
	/// <returns><c>true</c> if the collection is valid; <c>false</c> otherwise.</returns>
	public static bool IsNullOrEmpty<T>(this ICollection<T>? source) => source is not { Count: > 0 };

	/// <summary>
	/// Determines whether a collection isn't <c>null</c> and isn't empty.
	/// </summary>
	/// <param name="source">The collection.</param>
	/// <returns><c>true</c> if the collection is valid; <c>false</c> otherwise.</returns>
	public static bool IsNotNullAndNotEmpty<T>(this ICollection<T>? source) => source is { Count: > 0 };

	/// <summary>
	/// Determines whether a collection is empty.
	/// </summary>
	/// <param name="source">The collection.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns><c>true</c> if the collection is valid; <c>false</c> otherwise.</returns>
	public static bool None<T>(this ICollection<T> source) => source.Count == 0;

	/// <summary>
	/// Determines whether a collection isn't empty.
	/// </summary>
	/// <param name="source">The collection.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns><c>true</c> if the collection is valid; <c>false</c> otherwise.</returns>
	public static bool Any<T>(this ICollection<T> source) => source.Count != 0;

	/// <summary>
	/// Determines whether a collection contains multiple items.
	/// </summary>
	/// <param name="source">The collection.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns><c>true</c> if the collection is valid; <c>false</c> otherwise.</returns>
	public static bool Many<T>(this ICollection<T> source) => source.Count > 1;

	/// <summary>
	/// Determines whether a collection contains exactly the <paramref name="count" /> of items.
	/// </summary>
	/// <param name="source">The collection.</param>
	/// <param name="count">The exact count of items expected in the collection.</param>
	/// <returns><c>true</c> if the collection is valid; <c>false</c> otherwise.</returns>
	public static bool Exactly<T>(this ICollection<T> source, int count) => source.Count == count;

	/// <summary>
	/// Determines whether a collection contains more than the <paramref name="count" /> of items.
	/// </summary>
	/// <param name="source">The collection.</param>
	/// <param name="count">The count of items to compare against the collection count.</param>
	/// <returns><c>true</c> if the collection is valid; <c>false</c> otherwise.</returns>
	public static bool MoreThan<T>(this ICollection<T> source, int count) => source.Count > count;

	/// <summary>
	/// Determines whether a collection contains less than the <paramref name="count" /> of items.
	/// </summary>
	/// <param name="source">The collection.</param>
	/// <param name="count">The count of items to compare against the collection count.</param>
	/// <returns><c>true</c> if the collection is valid; <c>false</c> otherwise.</returns>
	public static bool LessThan<T>(this ICollection<T> source, int count) => source.Count < count;

	/// <summary>
	/// Determines whether a collection contains at least the <paramref name="count" /> of items.
	/// </summary>
	/// <param name="source">The collection.</param>
	/// <param name="count">The minimum count of items expected in the collection.</param>
	/// <returns><c>true</c> if the collection is valid; <c>false</c> otherwise.</returns>
	public static bool AtLeast<T>(this ICollection<T> source, int count) => source.Count >= count;

	/// <summary>
	/// Adds <paramref name="items" /> to a collection using the collection initializer syntax.
	/// </summary>
	/// <param name="source">The collection.</param>
	/// <param name="items">The items.</param>
	/// <typeparam name="T">Type of the <paramref name="items" />.</typeparam>
	public static void Add<T>(this ICollection<T> source, IEnumerable<T> items)
	{
		foreach (var item in items)
		{
			source.Add(item);
		}
	}

	/// <summary>
	/// Adds an <paramref name="item" /> to a collection and returns the <paramref name="item" />.
	/// </summary>
	/// <param name="source">The collection.</param>
	/// <param name="item">The item.</param>
	/// <typeparam name="T">Type of the item.</typeparam>
	/// <returns>The <paramref name="item" />.</returns>
	public static T AddAndReturn<T>(this ICollection<T> source, T item)
	{
		source.Add(item);
		return item;
	}

	/// <summary>
	/// Adds an <paramref name="item" /> to a collection and returns the collection.
	/// </summary>
	/// <param name="source">The collection.</param>
	/// <param name="item">The item.</param>
	/// <typeparam name="T">Type of the item.</typeparam>
	/// <returns>The collection.</returns>
	public static ICollection<T> AddAndReturnCollection<T>(this ICollection<T> source, T item)
	{
		source.Add(item);
		return source;
	}

	/// <summary>
	/// Creates an infinite cycle through the items of a collection.
	/// </summary>
	/// <param name="source">The collection to cycle through.</param>
	/// <param name="ct">The cancellation token.</param>
	/// <typeparam name="T">The type of items in the collection.</typeparam>
	/// <returns>An infinite enumerable of items from the <paramref name="source" />.</returns>
	public static IEnumerable<T> Cycle<T>(this ICollection<T> source, CancellationToken ct)
	{
		if (source.Count <= 0)
		{
			yield break;
		}

		while (!ct.IsCancellationRequested)
		{
			foreach (var w in source)
			{
				if (ct.IsCancellationRequested)
				{
					yield break;
				}

				yield return w;
			}
		}
	}
}