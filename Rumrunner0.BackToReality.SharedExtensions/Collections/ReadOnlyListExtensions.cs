using System;
using System.Collections.Generic;

namespace Rumrunner0.BackToReality.SharedExtensions.Collections;

/// <summary>Extensions for <see cref="IReadOnlyList{T}" />.</summary>
public static class ReadOnlyListExtensions
{
	/// <summary>Returns the first item in <paramref name="source" />.</summary>
	/// <param name="source">The collection.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns>The first item.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="source" /> is empty.</exception>
	public static T First<T>(this IReadOnlyList<T> source) => source[Index.FromStart(0)];

	/// <summary>Returns the last item in <paramref name="source" />.</summary>
	/// <param name="source">The collection.</param>
	/// <typeparam name="T">Type of the collection items.</typeparam>
	/// <returns>The last item.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="source" /> is empty.</exception>
	public static T Last<T>(this IReadOnlyList<T> source) => source[Index.FromEnd(1)];

	/// <summary>Deconstructs a list into a value tuple of two items.</summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="s" /> has fewer than two items.</exception>
	public static (T, T) Deconstruct2<T>(this IReadOnlyList<T> s) => (s[0], s[1]);

	/// <summary>Deconstructs a list into a value tuple of three items.</summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="s" /> has fewer than three items.</exception>
	public static (T, T, T) Deconstruct3<T>(this IReadOnlyList<T> s) => (s[0], s[1], s[2]);

	/// <summary>Deconstructs a list into a value tuple of four items.</summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="s" /> has fewer than four items.</exception>
	public static (T, T, T, T) Deconstruct4<T>(this IReadOnlyList<T> s) => (s[0], s[1], s[2], s[3]);

	/// <summary>Deconstructs a list into a value tuple of five items.</summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="s" /> has fewer than five items.</exception>
	public static (T, T, T, T, T) Deconstruct5<T>(this IReadOnlyList<T> s) => (s[0], s[1], s[2], s[3], s[4]);

	/// <summary>Deconstructs a list into a value tuple of six items.</summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="s" /> has fewer than six items.</exception>
	public static (T, T, T, T, T, T) Deconstruct6<T>(this IReadOnlyList<T> s) => (s[0], s[1], s[2], s[3], s[4], s[5]);

	/// <summary>Deconstructs a list into a value tuple of seven items.</summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="s" /> has fewer than seven items.</exception>
	public static (T, T, T, T, T, T, T) Deconstruct7<T>(this IReadOnlyList<T> s) => (s[0], s[1], s[2], s[3], s[4], s[5], s[6]);

	/// <summary>Deconstructs a list into a value tuple of eight items.</summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="s" /> has fewer than eight items.</exception>
	public static (T, T, T, T, T, T, T, T) Deconstruct8<T>(this IReadOnlyList<T> s) => (s[0], s[1], s[2], s[3], s[4], s[5], s[6], s[7]);
}