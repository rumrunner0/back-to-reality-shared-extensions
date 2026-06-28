using System;
using System.Collections.Generic;
using System.Text;

namespace Rumrunner0.BackToReality.SharedExtensions.Exceptions;

/// <summary>Extensions for <see cref="Exception" />.</summary>
public static class ExceptionExtensions
{
	/// <summary>
	/// Joins all messages in an <paramref name="source" /> exception. <br />
	/// This means that messages of all inner exceptions will be joined, including every branch of an <see cref="AggregateException" />.
	/// </summary>
	/// <param name="source">The exception.</param>
	/// <param name="separator">The separator.</param>
	/// <returns>A new string of joined messages.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> or <paramref name="separator" /> are <c>null</c>.</exception>
	/// <exception cref="ArgumentException">Thrown if <paramref name="separator" /> is empty.</exception>
	/// <remarks>The message of an <see cref="AggregateException" /> itself may already summarize its direct children, so parts of it can appear twice in the result.</remarks>
	public static string JoinMessages(this Exception source, string separator)
	{
		ArgumentExceptionExtensions.ThrowIfNull(source);
		ArgumentExceptionExtensions.ThrowIfNullOrEmpty(separator);

		var builder = new StringBuilder();
		AppendMessages(source, separator, builder);
		return builder.ToString();
	}

	/// <summary>Finds the first inner <typeparamref name="TException" /> in the chain of the <paramref name="source" /> exception, including every branch of an <see cref="AggregateException" />.</summary>
	/// <param name="source">The exception to check within.</param>
	/// <typeparam name="TException">The exception type to find.</typeparam>
	/// <returns>The first <typeparamref name="TException" /> instance found; <c>null</c> otherwise.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	public static TException? HasInner<TException>(this Exception source) where TException : Exception
	{
		ArgumentExceptionExtensions.ThrowIfNull(source);
		return FindInInner<TException>(source);
	}

	/// <summary>Finds the first <typeparamref name="TException" /> that the <paramref name="source" /> exception is or contains in the chain, including every branch of an <see cref="AggregateException" />.</summary>
	/// <param name="source">The exception to check within.</param>
	/// <typeparam name="TException">The exception type to find.</typeparam>
	/// <returns>The first <typeparamref name="TException" /> instance found; <c>null</c> otherwise.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	public static TException? IsOrHasInner<TException>(this Exception source) where TException : Exception
	{
		ArgumentExceptionExtensions.ThrowIfNull(source);

		if (source is TException target) return target;
		return FindInInner<TException>(source);
	}

	/// <summary>Appends the messages of an <paramref name="source" /> exception and all its inner exceptions to the <paramref name="builder" />.</summary>
	/// <param name="source">The exception.</param>
	/// <param name="separator">The separator.</param>
	/// <param name="builder">The builder.</param>
	private static void AppendMessages(Exception source, string separator, StringBuilder builder)
	{
		var stack = new Stack<Exception>();
		stack.Push(source);

		while (stack.Count > 0)
		{
			var current = stack.Pop();

			if (builder.Length > 0) builder.Append(separator);
			builder.Append(current.Message);

			if (current is AggregateException aggregate)
			{
				// Pushes the branches in reverse so they pop in the original order.
				var inners = aggregate.InnerExceptions;
				for (var i = inners.Count - 1; i >= 0; i--)
				{
					stack.Push(inners[i]);
				}

				continue;
			}

			if (current.InnerException is not null)
			{
				stack.Push(current.InnerException);
			}
		}
	}

	/// <summary>Finds the first <typeparamref name="TException" /> in the inner exceptions of the <paramref name="source" /> exception.</summary>
	/// <param name="source">The exception to check within.</param>
	/// <typeparam name="TException">The exception type to find.</typeparam>
	/// <returns>The first <typeparamref name="TException" /> instance found; <c>null</c> otherwise.</returns>
	private static TException? FindInInner<TException>(Exception source) where TException : Exception
	{
		if (source is AggregateException aggregate)
		{
			foreach (var inner in aggregate.InnerExceptions)
			{
				var found = inner.IsOrHasInner<TException>();
				if (found is not null) return found;
			}

			return null;
		}

		return source.InnerException?.IsOrHasInner<TException>();
	}
}