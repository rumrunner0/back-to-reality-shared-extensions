using System.Collections.Generic;
using System.Threading;

namespace Rumrunner0.BackToReality.SharedExtensions.Collections;

/// <summary>Extensions for <see cref="ICollection{T}" />.</summary>
public static class CollectionExtensions
{
	/// <summary>Adds <paramref name="items" /> to a collection using the collection initializer syntax.</summary>
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

	/// <summary>Adds an <paramref name="item" /> to a collection and returns the <paramref name="item" />.</summary>
	/// <param name="source">The collection.</param>
	/// <param name="item">The item.</param>
	/// <typeparam name="T">Type of the item.</typeparam>
	/// <returns>The <paramref name="item" />.</returns>
	public static T AddAndReturn<T>(this ICollection<T> source, T item)
	{
		source.Add(item);
		return item;
	}

	/// <summary>Adds an <paramref name="item" /> to a collection and returns the collection.</summary>
	/// <param name="source">The collection.</param>
	/// <param name="item">The item.</param>
	/// <typeparam name="T">Type of the item.</typeparam>
	/// <returns>The collection.</returns>
	public static ICollection<T> AddAndReturnCollection<T>(this ICollection<T> source, T item)
	{
		source.Add(item);
		return source;
	}

	/// <summary>Creates an infinite cycle through the items of a collection.</summary>
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