using System;
using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Rumrunner0.BackToReality.SharedExtensions.Exceptions;

namespace Rumrunner0.BackToReality.SharedExtensions.Extensions;

/// <summary>Extensions for <see cref="string" />.</summary>
public static class StringExtensions
{
	/// <summary>Determines whether a <paramref name="source" /> is <c>null</c> or an empty string.</summary>
	/// <param name="source">The string.</param>
	/// <returns><c>true</c> if the <paramref name="source" /> is <c>null</c> or empty; <c>false</c> otherwise.</returns>
	public static bool IsNullOrEmpty([NotNullWhen(false)] this string? source)
	{
		return string.IsNullOrEmpty(source);
	}

	/// <summary>Determines whether a <paramref name="source" /> is an empty string.</summary>
	/// <param name="source">The string.</param>
	/// <returns><c>true</c> if the <paramref name="source" /> is empty; <c>false</c> otherwise.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	public static bool IsEmpty(this string source)
	{
		ArgumentExceptionExtensions.ThrowIfNull(source);
		return source.Length == 0;
	}

	/// <summary>Determines whether a <paramref name="source"/> is <c>null</c>, empty, or consists only of whitespace characters.</summary>
	/// <param name="source">The string.</param>
	/// <returns><c>true</c> if the <paramref name="source" /> is <c>null</c>, empty, or whitespace; <c>false</c> otherwise.</returns>
	public static bool IsNullOrEmptyOrWhitespace([NotNullWhen(false)] this string? source)
	{
		return string.IsNullOrWhiteSpace(source);
	}

	/// <summary>Determines whether a <paramref name="source" /> is an empty string or contains only whitespace characters.</summary>
	/// <param name="source">The string.</param>
	/// <returns><c>true</c> if the <paramref name="source" /> is empty or whitespace; <c>false</c> otherwise.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	public static bool IsEmptyOrWhitespace(this string source)
	{
		ArgumentExceptionExtensions.ThrowIfNull(source);
		return source.AsSpan().IsWhiteSpace();
	}

	/// <summary>Determines whether a <paramref name="value" /> is a valid JSON.</summary>
	/// <param name="value">The value to be validated.</param>
	/// <returns><c>true</c> if the <paramref name="value" /> is a valid JSON; <c>false</c> otherwise.</returns>
	public static bool IsValidJson([NotNullWhen(true)] this string? value)
	{
		if (value is null || value.IsEmptyOrWhitespace())
		{
			return false;
		}

		try
		{
			using var document = JsonDocument.Parse(value);
			return true;
		}
		catch (JsonException)
		{
			return false;
		}
	}

	/// <summary>Splits a string into substrings based on a whitespace delimiter.</summary>
	/// <param name="source">The string.</param>
	/// <returns>An array of substrings.</returns>
	public static string[] SplitByWhitespace(this string source)
	{
		return source.Split(default(string[]?), StringSplitOptions.RemoveEmptyEntries);
	}

	/// <summary>
	/// Formats a <paramref name="source" /> with <paramref name="values" />. <br />
	/// Basically, this is a wrapper on <see cref="string" />.<see cref="string.Format(string,object?[])" />
	/// </summary>
	/// <param name="source">The string.</param>
	/// <param name="values">The values.</param>
	/// <returns>A new formatted string.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> or <paramref name="values" /> is <c>null</c>.</exception>
	/// <exception cref="FormatException">Thrown if <paramref name="source" /> is not a valid composite format string.</exception>
	public static string Format(this string source, params object?[] values)
	{
		return string.Format(source, values);
	}

	/// <summary>Parses a string to the <see cref="Guid" />.</summary>
	/// <param name="source">The value to be parsed.</param>
	/// <returns>A <see cref="Guid" /> created from the parsed <paramref name="source" />.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
	/// <exception cref="FormatException"><paramref name="source" /> is not in a recognized format.</exception>
	public static Guid ToGuid(this string source)
	{
		return Guid.Parse(source);
	}

	/// <summary>Tries to parse a string to the <see cref="Guid" />.</summary>
	/// <param name="source">The value to be parsed.</param>
	/// <returns>A new <see cref="Guid" /> if <paramref name="source" /> is valid; <c>null</c> otherwise.</returns>
	public static Guid? ToGuidOrNull(this string? source)
	{
		return source is not null && Guid.TryParse(source, out var result) ? result : null;
	}

	/// <summary>Tries to decode the <paramref name="base64String" />.</summary>
	/// <param name="base64String">The string encoded using Base64.</param>
	/// <param name="bytes">The decoded bytes or an empty array if the conversion fails.</param>
	/// <returns><c>true</c> if the conversion was successful; <c>false</c> otherwise.</returns>
	public static bool TryGetBytesFromBase64String([NotNullWhen(true)] string? base64String, out byte[] bytes)
	{
		if (base64String is null || !Base64.IsValid(base64String, out var decodedLength))
		{
			bytes = [];
			return false;
		}

		bytes = new byte[decodedLength];
		return Convert.TryFromBase64String(base64String, bytes, out _);
	}
}