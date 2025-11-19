using System.Collections.Generic;

namespace Rumrunner0.BackToReality.SharedExtensions.Extensions;

/// <summary>Extensions for arrays.</summary>
public static class ArrayExtensions
{
	/// <summary>
	/// Creates an array containing only the non-null values from <paramref name="items" />.
	/// </summary>
	/// <param name="items">The items.</param>
	/// <typeparam name="T">The type of items.</typeparam>
	/// <returns>A new array.</returns>
	public static T[] CreateFromNonNulls<T>(params IEnumerable<T?> items) where T : struct
	{
		var list = new List<T>();
		list.AddNonNulls(items);
		return list.ToArray();
	}

	/// <summary>
	/// Creates an array from non-null <paramref name="items" />.
	/// </summary>
	/// <param name="items">The items.</param>
	/// <typeparam name="T">The type of items.</typeparam>
	/// <returns>A new array.</returns>
	public static T[] CreateFromNonNulls<T>(params IEnumerable<T?> items) where T : class
	{
		var list = new List<T>();
		list.AddNonNulls(items);
		return list.ToArray();
	}
}