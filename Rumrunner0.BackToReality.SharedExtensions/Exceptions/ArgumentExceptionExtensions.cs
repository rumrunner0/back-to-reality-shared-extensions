using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Rumrunner0.BackToReality.SharedExtensions.Extensions;

namespace Rumrunner0.BackToReality.SharedExtensions.Exceptions;

/// <summary>Extensions for <see cref="ArgumentException" />.</summary>
public static class ArgumentExceptionExtensions
{
	/// <summary>Throws an <see cref="ArgumentNullException" /> if <paramref name="source" /> is <c>null</c>.</summary>
	/// <param name="source">The object to validate.</param>
	/// <param name="argumentName">The name of the <paramref name="source" /> argument.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	public static void ThrowIfNull(object? source, [CallerArgumentExpression("source")] string? argumentName = null)
	{
		if (source is null) throw new ArgumentNullException(argumentName);
	}

	#region Collections

	/// <summary>Throws an exception if <paramref name="source" /> is <c>null</c> or empty.</summary>
	/// <param name="source">The collection to validate.</param>
	/// <param name="argumentName">The name of the <paramref name="source" /> argument.</param>
	/// <typeparam name="T">The type of <paramref name="source" /> collection items.</typeparam>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is <c>null</c>.</exception>
	/// <exception cref="ArgumentException">Thrown if <paramref name="source" /> is empty.</exception>
	public static void ThrowIfNullOrEmpty<T>(ICollection<T>? source, [CallerArgumentExpression("source")] string? argumentName = null)
	{
		ArgumentExceptionExtensions.ThrowIfNull(source, argumentName);
		ArgumentExceptionExtensions.ThrowIfEmpty(source!, argumentName);
	}

	/// <summary>Throws an <see cref="ArgumentException" /> if <paramref name="source" /> is empty.</summary>
	/// <param name="source">The collection to validate.</param>
	/// <param name="argumentName">The name of the <paramref name="source" /> argument.</param>
	/// <typeparam name="T">The type of the <paramref name="source" /> collection items.</typeparam>
	/// <exception cref="ArgumentException">Thrown if <paramref name="source" /> is empty.</exception>
	/// <remarks>This method DOESN'T throw, if <paramref name="source" /> is <c>null</c>! If you need to check for <c>null</c> as well, use <see cref="ThrowIfNullOrEmpty{T}" /> instead.</remarks>
	public static void ThrowIfEmpty<T>(ICollection<T> source, [CallerArgumentExpression("source")] string? argumentName = null)
	{
		if (source is null) return;
		if (source.None()) throw new ArgumentException($"{argumentName} is empty");
	}

	#endregion

	#region Strings

	/// <summary>Throws an exception if <paramref name="source" /> is empty.</summary>
	/// <param name="source">The <see cref="string" /> to validate.</param>
	/// <param name="argumentName">The name of the <paramref name="source" /> argument.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is null.</exception>
	/// <exception cref="ArgumentException">Thrown if <paramref name="source" /> is empty or whitespace.</exception>
	public static void ThrowIfNullOrEmptyOrWhiteSpace(string? source, [CallerArgumentExpression("source")] string? argumentName = null)
	{
		ArgumentExceptionExtensions.ThrowIfNull(source, argumentName);
		ArgumentExceptionExtensions.ThrowIfEmptyOrWhiteSpace(source!, argumentName);
	}

	/// <summary>Throws an <see cref="ArgumentException" /> if <paramref name="source" /> is empty or whitespace.</summary>
	/// <param name="source">The <see cref="string" /> to validate.</param>
	/// <param name="argumentName">The name of the <paramref name="source" /> argument.</param>
	/// <exception cref="ArgumentException">Thrown if <paramref name="source" /> is empty or whitespace.</exception>
	/// <remarks>This method DOESN'T throw, if <paramref name="source" /> is <c>null</c>! If you need to check for <c>null</c> as well, use <see cref="ThrowIfNullOrEmptyOrWhiteSpace" /> instead.</remarks>
	public static void ThrowIfEmptyOrWhiteSpace(string source, [CallerArgumentExpression("source")] string? argumentName = null)
	{
		if (source is null) return;
		if (source.IsEmptyOrWhitespace()) throw new ArgumentException($"{argumentName} is empty or whitespace");
	}

	/// <summary>Throws an exception if <paramref name="source" /> is <c>null</c> or empty.</summary>
	/// <param name="source">The <see cref="string" /> to validate.</param>
	/// <param name="argumentName">The name of the <paramref name="source" /> argument.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> is null.</exception>
	/// <exception cref="ArgumentException">Thrown if <paramref name="source" /> is empty.</exception>
	public static void ThrowIfNullOrEmpty(string source, [CallerArgumentExpression("source")] string? argumentName = null)
	{
		ArgumentExceptionExtensions.ThrowIfNull(source, argumentName);
		ArgumentExceptionExtensions.ThrowIfEmpty(source, argumentName);
	}

	/// <summary>Throws an <see cref="ArgumentException" /> if <paramref name="source" /> is empty.</summary>
	/// <param name="source">The <see cref="string" /> to validate.</param>
	/// <param name="argumentName">The name of the <paramref name="source" /> argument.</param>
	/// <exception cref="ArgumentException">Thrown if <paramref name="source" /> is empty.</exception>
	/// <remarks>This method DOESN'T throw, if <paramref name="source" /> is <c>null</c>! If you need to check for <c>null</c> as well, use <see cref="ThrowIfNullOrEmpty" /> instead.</remarks>
	public static void ThrowIfEmpty(string source, [CallerArgumentExpression("source")] string? argumentName = null)
	{
		if (source is null) return;
		if (source.IsEmpty()) throw new ArgumentException($"{argumentName} is empty");
	}

	#endregion
}