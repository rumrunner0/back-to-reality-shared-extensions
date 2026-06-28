using System;
using System.Collections.Generic;

namespace Rumrunner0.BackToReality.SharedExtensions.Collections;

/// <summary>Extensions for <see cref="IList{T}" /> and <see cref="List{T}" />.</summary>
public static class ListExtensions
{
	/// <summary>Returns the first item in <paramref name="source" />.</summary>
	/// <param name="source">The collection.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns>The first item.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="source" /> is empty.</exception>
	public static T First<T>(this IList<T> source) => source[Index.FromStart(0)];

	/// <summary>Returns the last item in <paramref name="source" />.</summary>
	/// <param name="source">The collection.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns>The last item.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="source" /> is empty.</exception>
	public static T Last<T>(this IList<T> source) => source[Index.FromEnd(1)];

	/// <summary>Adds <paramref name="items" /> to a list.</summary>
	/// <remarks>Makes it possible to use the <b>collection initializer syntax</b> because of naming.</remarks>
	/// <param name="source">The list.</param>
	/// <param name="items">The items.</param>
	/// <typeparam name="T">Type of the list items.</typeparam>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="items" /> is <c>null</c>.</exception>
	public static void Add<T>(this List<T> source, IEnumerable<T> items) => source.AddRange(items);

	/// <summary>
	/// Adds the non-null <paramref name="items" /> to the <paramref name="source" /> list.
	/// </summary>
	/// <param name="source">The collection.</param>
	/// <param name="items">The items.</param>
	/// <typeparam name="T">The list item type.</typeparam>
	/// <returns>The same <see cref="List{T}" />.</returns>
	public static List<T> AddNonNulls<T>(this List<T> source, params IEnumerable<T?> items) where T : struct
	{
		foreach (var item in items)
		{
			if (!item.HasValue) continue;
			source.Add(item.Value);
		}

		return source;
	}

	/// <summary>
	/// Adds the non-null <paramref name="items" /> to the <paramref name="source" /> list.
	/// </summary>
	/// <param name="source">The collection.</param>
	/// <param name="items">The items.</param>
	/// <typeparam name="T">The list item type.</typeparam>
	/// <returns>The same <see cref="List{T}" />.</returns>
	public static List<T> AddNonNulls<T>(this List<T> source, params IEnumerable<T?> items) where T : class
	{
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
		source.RemoveRange(index: start, count: end - start + 1);
		return source;
	}

	/// <summary>Deconstructs a list into a value tuple of two items.</summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="s" /> has fewer than two items.</exception>
	public static (T, T) Deconstruct2<T>(this IList<T> s) => (s[0], s[1]);

	/// <summary>Deconstructs a list into a value tuple of three items.</summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="s" /> has fewer than three items.</exception>
	public static (T, T, T) Deconstruct3<T>(this IList<T> s) => (s[0], s[1], s[2]);

	/// <summary>Deconstructs a list into a value tuple of four items.</summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="s" /> has fewer than four items.</exception>
	public static (T, T, T, T) Deconstruct4<T>(this IList<T> s) => (s[0], s[1], s[2], s[3]);

	/// <summary>Deconstructs a list into a value tuple of five items.</summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="s" /> has fewer than five items.</exception>
	public static (T, T, T, T, T) Deconstruct5<T>(this IList<T> s) => (s[0], s[1], s[2], s[3], s[4]);

	/// <summary>Deconstructs a list into a value tuple of six items.</summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="s" /> has fewer than six items.</exception>
	public static (T, T, T, T, T, T) Deconstruct6<T>(this IList<T> s) => (s[0], s[1], s[2], s[3], s[4], s[5]);

	/// <summary>Deconstructs a list into a value tuple of sever items.</summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="s" /> has fewer than seven items.</exception>
	public static (T, T, T, T, T, T, T) Deconstruct7<T>(this IList<T> s) => (s[0], s[1], s[2], s[3], s[4], s[5], s[6]);

	/// <summary>Deconstructs a list into a value tuple of eight items.</summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="s" /> has fewer than eight items.</exception>
	public static (T, T, T, T, T, T, T, T) Deconstruct8<T>(this IList<T> s) => (s[0], s[1], s[2], s[3], s[4], s[5], s[6], s[7]);
}