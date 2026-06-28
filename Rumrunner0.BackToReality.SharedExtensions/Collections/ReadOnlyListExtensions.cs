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
}