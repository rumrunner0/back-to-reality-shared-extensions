using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Rumrunner0.BackToReality.SharedExtensions.Collections;

/// <summary>Extensions for <see cref="IEnumerable{T}" />.</summary>
public static class EnumerableExtensions
{
	/// <summary>Joins a collection using <paramref name="separator" />.</summary>
	/// <param name="source">The collection.</param>
	/// <param name="separator">The separator.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns>A new string that contains joined collection items.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	public static string StringJoin<T>(this IEnumerable<T> source, string separator = " ")
	{
		return string.Join(separator, source);
	}

	/// <summary>Determines whether a collection is <c>null</c> or empty.</summary>
	/// <param name="source">The collection.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns><c>true</c> if the collection is valid; <c>false</c> otherwise.</returns>
	public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? source)
	{
		return source is null || source.None();
	}

	/// <summary>Determines whether a collection isn't <c>null</c> and isn't empty.</summary>
	/// <param name="source">The collection.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns><c>true</c> if the collection is valid; <c>false</c> otherwise.</returns>
	public static bool IsNotNullAndNotEmpty<T>([NotNullWhen(true)] this IEnumerable<T>? source)
	{
		return source is not null && source.Some();
	}

	/// <summary>Determines whether a collection is empty.</summary>
	/// <param name="source">The collection.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns><c>true</c> if the collection is empty; <c>false</c> otherwise.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	public static bool None<T>(this IEnumerable<T> source)
	{
		return source.Exactly(0);
	}

	/// <summary>Determines whether a collection isn't empty.</summary>
	/// <param name="source">The collection.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns><c>true</c> if the collection has at least one item; <c>false</c> otherwise.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	public static bool Some<T>(this IEnumerable<T> source)
	{
		return source.AtLeast(1);
	}

	/// <summary>Determines whether a collection contains multiple items.</summary>
	/// <param name="source">The collection.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns><c>true</c> if the collection has multiple items; <c>false</c> otherwise.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	public static bool Many<T>(this IEnumerable<T> source)
	{
		return source.MoreThan(1);
	}

	/// <summary>Determines whether a collection contains exactly the <paramref name="count" /> of items.</summary>
	/// <param name="source">The collection.</param>
	/// <param name="count">The exact count of items expected in the collection.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns><c>true</c> if the collection is valid; <c>false</c> otherwise.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	public static bool Exactly<T>(this IEnumerable<T> source, int count)
	{
		return CountAtMost(source, count < 0 ? 0 : count + 1) == count;
	}

	/// <summary>Determines whether a collection contains more than the <paramref name="count" /> of items.</summary>
	/// <param name="source">The collection.</param>
	/// <param name="count">The count of items to compare against the collection count.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns><c>true</c> if the collection is valid; <c>false</c> otherwise.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	public static bool MoreThan<T>(this IEnumerable<T> source, int count)
	{
		return CountAtMost(source, count < 0 ? 0 : count + 1) > count;
	}

	/// <summary>Determines whether a collection contains less than the <paramref name="count" /> of items.</summary>
	/// <param name="source">The collection.</param>
	/// <param name="count">The count of items to compare against the collection count.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns><c>true</c> if the collection is valid; <c>false</c> otherwise.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	public static bool LessThan<T>(this IEnumerable<T> source, int count)
	{
		return CountAtMost(source, count < 0 ? 0 : count) < count;
	}

	/// <summary>Determines whether a collection contains at least the <paramref name="count" /> of items.</summary>
	/// <param name="source">The collection.</param>
	/// <param name="count">The minimum count of items expected in the collection.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns><c>true</c> if the collection is valid; <c>false</c> otherwise.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	public static bool AtLeast<T>(this IEnumerable<T> source, int count)
	{
		return CountAtMost(source, count < 0 ? 0 : count) >= count;
	}

	/// <summary>Counts the items in <paramref name="source" />, enumerating no further than <paramref name="limit" /> items.</summary>
	/// <param name="source">The collection.</param>
	/// <param name="limit">The maximum number of items to count.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns>The item count, capped at <paramref name="limit" />.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	private static int CountAtMost<T>(IEnumerable<T> source, int limit)
	{
		if (source.TryGetNonEnumeratedCount(out var count))
		{
			return count < limit ? count : limit;
		}

		var counted = 0;
		using var enumerator = source.GetEnumerator();
		while (counted < limit && enumerator.MoveNext())
		{
			counted++;
		}

		return counted;
	}
}