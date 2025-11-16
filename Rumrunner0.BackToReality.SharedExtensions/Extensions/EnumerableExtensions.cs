using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rumrunner0.BackToReality.SharedExtensions.Extensions;

/// <summary>Extensions for <see cref="IEnumerable{T}" />.</summary>
public static class EnumerableExtensions
{
	/// <summary>Joins a collection using <paramref name="separator" />.</summary>
	/// <param name="source">The collection.</param>
	/// <param name="separator">The separator.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns>A new string that contains joined collection items.</returns>
	public static string StringJoin<T>(this IEnumerable<T> source, string separator = " ")
	{
		return string.Join(separator, source);
	}

	/// <summary>Determines whether a collection is empty.</summary>
	/// <param name="source">The collection.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns><c>true</c> if the collection is empty; <c>false</c> otherwise.</returns>
	public static bool None<T>(this IEnumerable<T> source)
	{
		return !source.Any();
	}

	/// <summary>Determines whether a collection contains multiple items.</summary>
	/// <param name="source">The collection.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns><c>true</c> if the collection has multiple items; <c>false</c> otherwise.</returns>
	public static bool Many<T>(this IEnumerable<T> source)
	{
		switch (source)
		{
			case null: throw new ArgumentNullException(nameof(source));
			case ICollection<T> c: return c.Count > 1;
			case ICollection c: return c.Count > 1;
		}

		using var enumerator = source.GetEnumerator();
		return enumerator.MoveNext() && enumerator.MoveNext();
	}
}