using System;

namespace Rumrunner0.BackToReality.SharedExtensions.Extensions;

/// <summary>
/// Extensions for object shaping.
/// </summary>
public static class ShaperExtensions
{
	/// <summary>
	/// Shapes an object into another object.
	/// </summary>
	/// <param name="source">The object to be shaped.</param>
	/// <param name="shaper">The shaper.</param>
	/// <typeparam name="TSource">Type of the object to be shaped.</typeparam>
	/// <typeparam name="TTarget">Type of the object to be shaped to.</typeparam>
	/// <returns>A new shape of the object.</returns>
	public static TTarget Shape<TSource, TTarget>(this TSource source, Func<TSource, TTarget> shaper)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(shaper);
		return shaper.Invoke(source);
	}

	/// <summary>
	/// Executes a <paramref name="node" /> action and returns a <paramref name="source" />.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="node">The action.</param>
	/// <typeparam name="TSource">Type of the source.</typeparam>
	/// <returns>The source.</returns>
	public static TSource? Chain<TSource>(this TSource? source, Action<TSource?> node)
	{
		ArgumentNullException.ThrowIfNull(node);
		node.Invoke(source);
		return source;
	}

	/// <summary>
	/// Makes a chain use the <paramref name="target" />.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="target">The target.</param>
	/// <typeparam name="TSource">Type of the source.</typeparam>
	/// <typeparam name="TTarget">Type of the target.</typeparam>
	/// <returns>The target object.</returns>
	public static TTarget? Follow<TSource, TTarget>(this TSource? source, TTarget? target)
	{
		return target;
	}
}