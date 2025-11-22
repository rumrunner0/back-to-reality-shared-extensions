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
	public static string JoinMessages(this Exception source, string separator)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentException.ThrowIfNullOrEmpty(separator);

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
}