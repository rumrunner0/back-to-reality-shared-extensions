using System;
using System.Runtime.CompilerServices;

namespace Rumrunner0.BackToReality.SharedExtensions.Exceptions;

/// <summary>Extensions for <see cref="ArgumentNullException" />.</summary>
internal static class ArgumentNullExceptionExtensions
{
	/// <summary>Throws an <see cref="ArgumentNullException" /> if <paramref name="source" /> is <c>null</c>.</summary>
	/// <param name="source">The object to validate.</param>
	/// <param name="argumentName">The name of the <paramref name="source" /> argument.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	internal static void ThrowIfNull(object? source, [CallerArgumentExpression("source")] string? argumentName = null)
	{
		if (source is null) throw new ArgumentNullException(argumentName);
	}
}