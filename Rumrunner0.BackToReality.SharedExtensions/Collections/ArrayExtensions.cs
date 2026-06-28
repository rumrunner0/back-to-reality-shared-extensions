using System;
using System.Collections.Generic;
using Rumrunner0.BackToReality.SharedExtensions.Exceptions;

namespace Rumrunner0.BackToReality.SharedExtensions.Collections;

/// <summary>Extensions for arrays.</summary>
public static class ArrayExtensions
{
	/// <summary>
	/// Creates an array containing only the non-null values from <paramref name="items" />.
	/// </summary>
	/// <param name="items">The items.</param>
	/// <typeparam name="T">The type of items.</typeparam>
	/// <returns>A new array.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="items" /> is <c>null</c>.</exception>
	public static T[] CreateFromNonNulls<T>(params IEnumerable<T?> items) where T : struct
	{
		ArgumentExceptionExtensions.ThrowIfNull(items);

		var list = new List<T>();
		list.AddNonNulls(items);
		return list.ToArray();
	}

	/// <summary>
	/// Creates an array containing only the non-null values from <paramref name="items" />.
	/// </summary>
	/// <param name="items">The items.</param>
	/// <typeparam name="T">The type of items.</typeparam>
	/// <returns>A new array.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="items" /> is <c>null</c>.</exception>
	public static T[] CreateFromNonNulls<T>(params IEnumerable<T?> items) where T : class
	{
		ArgumentExceptionExtensions.ThrowIfNull(items);

		var list = new List<T>();
		list.AddNonNulls(items);
		return list.ToArray();
	}
}
