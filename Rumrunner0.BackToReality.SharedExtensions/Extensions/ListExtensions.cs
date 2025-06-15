using System.Collections.Generic;

namespace Rumrunner0.BackToReality.SharedExtensions.Extensions;

/// <summary>
/// Extensions for <see cref="List{T}" />.
/// </summary>
public static class ListExtensions
{
	/// <summary>
	/// Adds <paramref name="items" /> to a list.
	/// </summary>
	/// <remarks>
	/// Makes it possible to use the <b>collection initializer syntax</b> because of naming.
	/// </remarks>
	/// <param name="source">The collection.</param>
	/// <param name="items">The items.</param>
	/// <typeparam name="T">Type of the list items.</typeparam>
	public static void Add<T>(this List<T> source, IEnumerable<T> items)
	{
		source.AddRange(items);
	}

	/// <summary>
	/// Deconstructs a list into a value tuple of two items.
	/// </summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	public static (T, T) Deconstruct2<T>(this IList<T> s) => (s[0], s[1]);

	/// <summary>
	/// Deconstructs a list into a value tuple of three items.
	/// </summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	public static (T, T, T) Deconstruct3<T>(this IList<T> s) => (s[0], s[1], s[2]);

	/// <summary>
	/// Deconstructs a list into a value tuple of four items.
	/// </summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	public static (T, T, T, T) Deconstruct4<T>(this IList<T> s) => (s[0], s[1], s[2], s[3]);

	/// <summary>
	/// Deconstructs a list into a value tuple of five items.
	/// </summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	public static (T, T, T, T, T) Deconstruct5<T>(this IList<T> s) => (s[0], s[1], s[2], s[3], s[4]);

	/// <summary>
	/// Deconstructs a list into a value tuple of six items.
	/// </summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	public static (T, T, T, T, T, T) Deconstruct6<T>(this IList<T> s) => (s[0], s[1], s[2], s[3], s[4], s[5]);

	/// <summary>
	/// Deconstructs a list into a value tuple of sever items.
	/// </summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	public static (T, T, T, T, T, T, T) Deconstruct7<T>(this IList<T> s) => (s[0], s[1], s[2], s[3], s[4], s[5], s[6]);

	/// <summary>
	/// Deconstructs a list into a value tuple of eight items.
	/// </summary>
	/// <param name="s">The list.</param>
	/// <typeparam name="T">The item type.</typeparam>
	/// <returns>New value tuple.</returns>
	public static (T, T, T, T, T, T, T, T) Deconstruct8<T>(this IList<T> s) => (s[0], s[1], s[2], s[3], s[4], s[5], s[6], s[7]);
}