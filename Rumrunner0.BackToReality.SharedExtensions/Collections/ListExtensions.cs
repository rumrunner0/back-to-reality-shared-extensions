using System;
using System.Collections.Generic;
using Rumrunner0.BackToReality.SharedExtensions.Exceptions;

namespace Rumrunner0.BackToReality.SharedExtensions.Collections;

/// <summary>Extensions for <see cref="IList{T}" /> and <see cref="List{T}" />.</summary>
public static class ListExtensions
{
	/// <summary>Adds <paramref name="items" /> to a list.</summary>
	/// <remarks>Makes it possible to use the <b>collection initializer syntax</b> because of naming.</remarks>
	/// <param name="source">The list.</param>
	/// <param name="items">The items.</param>
	/// <typeparam name="T">Type of the list items.</typeparam>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="items" /> is <c>null</c>.</exception>
	public static void Add<T>(this List<T> source, IEnumerable<T> items) => source.AddRange(items);

	/// <summary>Adds the non-null <paramref name="items" /> to the <paramref name="source" /> list.</summary>
	/// <param name="source">The collection.</param>
	/// <param name="items">The items.</param>
	/// <typeparam name="T">The list item type.</typeparam>
	/// <returns>The same <see cref="List{T}" />.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="items" /> is <c>null</c>.</exception>
	public static List<T> AddNonNulls<T>(this List<T> source, params IEnumerable<T?> items) where T : struct
	{
		ArgumentExceptionExtensions.ThrowIfNull(items);

		foreach (var item in items)
		{
			if (!item.HasValue) continue;
			source.Add(item.Value);
		}

		return source;
	}

	/// <summary>Adds the non-null <paramref name="items" /> to the <paramref name="source" /> list.</summary>
	/// <param name="source">The collection.</param>
	/// <param name="items">The items.</param>
	/// <typeparam name="T">The list item type.</typeparam>
	/// <returns>The same <see cref="List{T}" />.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="items" /> is <c>null</c>.</exception>
	public static List<T> AddNonNulls<T>(this List<T> source, params IEnumerable<T?> items) where T : class
	{
		ArgumentExceptionExtensions.ThrowIfNull(items);

		foreach (var item in items)
		{
			if (item is null) continue;
			source.Add(item);
		}

		return source;
	}

	/// <summary>Removes the first item from <paramref name="source"/>.</summary>
	/// <param name="source">The collection.</param>
	/// <typeparam name="T">The list item type.</typeparam>
	/// <returns>The same <see cref="List{T}" />.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="source" /> is empty.</exception>
	public static List<T> RemoveFirst<T>(this List<T> source)
	{
		source.RemoveAt(0);
		return source;
	}

	/// <summary>Removes the last item from <paramref name="source"/>.</summary>
	/// <param name="source">The collection.</param>
	/// <typeparam name="T">The list item type.</typeparam>
	/// <returns>The same <see cref="List{T}" />.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="source" /> is empty.</exception>
	public static List<T> RemoveLast<T>(this List<T> source)
	{
		source.RemoveAt(source.Count - 1);
		return source;
	}

	/// <summary>Removes items from <paramref name="source"/> at index <paramref name="start"/> through the last item, inclusive.</summary>
	/// <param name="source">The collection.</param>
	/// <param name="start">The start index.</param>
	/// <typeparam name="T">The list item type.</typeparam>
	/// <returns>The same <see cref="List{T}" />.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="start" /> is out of range.</exception>
	public static List<T> RemoveToEnd<T>(this List<T> source, int start)
	{
		if (start < 0 || start >= source.Count) throw new ArgumentOutOfRangeException(nameof(start), start, "Start must be within the bounds of the list");
		source.RemoveBetween(start: start, end: source.Count - 1);
		return source;
	}

	/// <summary>Removes items from <paramref name="source"/> from the first item through index <paramref name="end"/>, inclusive.</summary>
	/// <param name="source">The collection.</param>
	/// <param name="end">The end index.</param>
	/// <typeparam name="T">The list item type.</typeparam>
	/// <returns>The same <see cref="List{T}" />.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="end" /> is negative.</exception>
	/// <exception cref="ArgumentException">Thrown if <paramref name="end" /> is beyond the last index of <paramref name="source" />.</exception>
	public static List<T> RemoveFromStart<T>(this List<T> source, int end)
	{
		source.RemoveBetween(start: 0, end: end);
		return source;
	}

	/// <summary>Removes items from <paramref name="source"/> from index <paramref name="start"/> through index <paramref name="end"/>, inclusive.</summary>
	/// <param name="source">The collection.</param>
	/// <param name="start">The start index, included.</param>
	/// <param name="end">The end index, included.</param>
	/// <typeparam name="T">The list item type.</typeparam>
	/// <returns>The same <see cref="List{T}" />.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="start" /> is negative or <paramref name="end" /> is less than <paramref name="start" />.</exception>
	/// <exception cref="ArgumentException">Thrown if the range from <paramref name="start" /> to <paramref name="end" /> is outside the bounds of <paramref name="source" />.</exception>
	public static List<T> RemoveBetween<T>(this List<T> source, int start, int end)
	{
		if (start < 0) throw new ArgumentOutOfRangeException(nameof(start), start, "Start must be non-negative");
		if (end < start) throw new ArgumentOutOfRangeException(nameof(end), end, "End must be greater than or equal to start");
		source.RemoveRange(index: start, count: end - start + 1);
		return source;
	}
}