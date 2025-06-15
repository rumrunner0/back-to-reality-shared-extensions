using System;
using System.Text;
using Humanizer;

namespace Rumrunner0.BackToReality.SharedExtensions.Extensions;

/// <summary>
/// Extensions for <see cref="Exception" />.
/// </summary>
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

	/// <summary>
	/// Humanizes the name of a <paramref name="source" /> exception.
	/// </summary>
	/// <param name="source">The exception.</param>
	/// <param name="includeEnding">The flag that indicates whether to include the "Exception" word.</param>
	/// <returns>A humanized name.</returns>
	public static string HumanizeName(this Exception source, bool includeEnding = false)
	{
		ArgumentNullException.ThrowIfNull(source);

		var name = source.GetType().Name.Humanize();

		if (!includeEnding)
		{
			const string EXCEPTION_WORD = "exception";
			var i = name.LastIndexOf(EXCEPTION_WORD, StringComparison.OrdinalIgnoreCase);
			if (i > 0 && i + EXCEPTION_WORD.Length == name.Length) name = name.Remove(startIndex: i);
		}

		return name.Trim();
	}
}