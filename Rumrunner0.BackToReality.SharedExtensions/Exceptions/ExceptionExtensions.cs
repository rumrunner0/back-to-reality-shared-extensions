using System;
using System.Text;

namespace Rumrunner0.BackToReality.SharedExtensions.Exceptions;

/// <summary>Extensions for <see cref="Exception" />.</summary>
public static class ExceptionExtensions
{
	/// <summary>
	/// Joins all messages in an <paramref name="source" /> exception. <br />
	/// This means that messages of all inner exceptions will be joined.
	/// </summary>
	/// <param name="source">The exception.</param>
	/// <param name="separator">The separator.</param>
	/// <returns>A new string of joined messages.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> or <paramref name="separator" /> are <c>null</c>.</exception>
	/// <exception cref="ArgumentException">Thrown if <paramref name="separator" /> is empty.</exception>
	public static string JoinMessages(this Exception source, string separator)
	{
		ArgumentExceptionExtensions.ThrowIfNull(source);
		ArgumentExceptionExtensions.ThrowIfNullOrEmpty(separator);

		var builder = new StringBuilder();
		builder.Append(source.Message);

		var inner = source.InnerException;
		while (inner is not null)
		{
			builder.Append(separator);
			builder.Append(inner.Message);
			inner = inner.InnerException;
		}

		return builder.ToString();
	}

	/// <summary>Determines whether the <paramref name="source" /> exception has inner <typeparamref name="TException" /> somewhere in the chain.</summary>
	/// <param name="source">The exception to check within.</param>
	/// <typeparam name="TException">The exception type to find.</typeparam>
	/// <returns>The first <typeparamref name="TException" /> instance found; <c>null</c>, otherwise.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	public static TException? HasInner<TException>(this Exception source) where TException : Exception
	{
		ArgumentExceptionExtensions.ThrowIfNull(source);
		return source.InnerException?.IsOrHasInner<TException>();
	}

	/// <summary>Determines whether the <paramref name="source" /> exception is <typeparamref name="TException" /> or contains it somewhere in the chain.</summary>
	/// <param name="source">The exception to check within.</param>
	/// <typeparam name="TException">The exception type to find.</typeparam>
	/// <returns>The first <typeparamref name="TException" /> instance found; <c>null</c>, otherwise.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	public static TException? IsOrHasInner<TException>(this Exception source) where TException : Exception
	{
		ArgumentExceptionExtensions.ThrowIfNull(source);

		var current = source;
		while (current is not null)
		{
			if (current is TException target) return target;
			current = current.InnerException;
		}

		return null;
	}
}