using System;
using System.Linq;
using System.Text.Json;

namespace Rumrunner0.BackToReality.SharedExtensions.Extensions;

/// <summary>
/// Extensions for <see cref="string" />.
/// </summary>
public static class StringExtensions
{
	/// <summary>
	/// Determines whether a <paramref name="source" /> is <c>null</c> or an empty string.
	/// </summary>
	/// <param name="source">The string.</param>
	/// <returns><c>true</c>, if the <paramref name="source" /> is valid, <c>false</c>, otherwise.</returns>
	public static bool IsNullOrEmpty(this string? source)
	{
		return string.IsNullOrEmpty(source);
	}

	/// <summary>
	/// Determines whether a <paramref name="source" /> is an empty string.
	/// </summary>
	/// <param name="source">The string.</param>
	/// <returns><c>true</c>, if the <paramref name="source" /> is valid, <c>false</c>, otherwise.</returns>
	public static bool IsEmpty(this string source)
	{
		ArgumentNullException.ThrowIfNull(source);
		return source.Length == 0;
	}

	/// <summary>
	/// Determines whether a <paramref name="source"/> is <c>null</c>, empty, or consists only of whitespace characters.
	/// </summary>
	/// <param name="source">The string.</param>
	/// <returns><c>true</c>, if the <paramref name="source" /> is valid, <c>false</c>, otherwise.</returns>
	public static bool IsNullOrEmptyOrWhitespace(this string? source)
	{
		return string.IsNullOrWhiteSpace(source);
	}

	/// <summary>
	/// Determines whether a <paramref name="source" /> is an empty string or contains only whitespaces.
	/// </summary>
	/// <param name="source">The string.</param>
	/// <returns><c>true</c>, if the <paramref name="source" /> is valid, <c>false</c>, otherwise.</returns>
	public static bool IsEmptyOrWhitespace(this string source)
	{
		ArgumentNullException.ThrowIfNull(source);
		return source.All(char.IsWhiteSpace);
	}

	/// <summary>
	/// Determines whether a <paramref name="value" /> is a valid JSON.
	/// </summary>
	/// <param name="value">The value to be validated.</param>
	/// <returns><c>true</c>, if the <paramref name="value" /> is a valid JSON, <c>false</c>, otherwise.</returns>
	public static bool IsValidJson(this string? value)
	{
		if (value is null || value.IsEmptyOrWhitespace())
		{
			return false;
		}

		try
		{
			_ = JsonDocument.Parse(value);
			return true;
		}
		catch
		{
			return false;
		}
	}

	/// <summary>
	/// Splits a string into substrings based on a whitespace delimiter.
	/// </summary>
	/// <param name="source">The string.</param>
	/// <returns>An array of substrings.</returns>
	public static string[] SplitByWhitespaces(this string source)
	{
		return source.Split(default(string[]?), StringSplitOptions.RemoveEmptyEntries);
	}

	/// <summary>
	/// Enriches a string with some data. <br />
	/// Basically, this is a wrapper on <see cref="string" />.<see cref="string.Format(string,object?[])" />
	/// </summary>
	/// <param name="source">The string.</param>
	/// <param name="values">The values.</param>
	/// <returns>A new enriched string.</returns>
	public static string Enrich(this string source, params object?[] values)
	{
		return string.Format(source, values);
	}

	/// <summary>
	/// Parses a string to the <see cref="Guid" />.
	/// </summary>
	/// <param name="source">The value to be parsed.</param>
	/// <returns>A <see cref="Guid" /> created from the parsed <paramref name="source" />.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
	/// <exception cref="FormatException"><paramref name="source" /> is not in a recognized format.</exception>
	public static Guid ToGuid(this string source)
	{
		return Guid.Parse(source);
	}

	/// <summary>
	/// Tries to parse a string to the <see cref="Guid" />.
	/// </summary>
	/// <param name="source">The value to be parsed.</param>
	/// <returns>A new <see cref="Guid" />, if <paramref name="source" /> is valid, <c>null</c>, otherwise.</returns>
	public static Guid? ToGuidOrNull(this string? source)
	{
		return source is null || !Guid.TryParse(source, out var result) ? null : result;
	}
}