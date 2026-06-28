using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Rumrunner0.BackToReality.SharedExtensions.Cryptography;

/// <summary>Extensions for <see cref="CryptographicOperations" />.</summary>
public static class CryptographicOperationExtensions
{
	/// <summary>Determines the equality of two strings, using ordinal comparison, in an amount of time that depends on the length of the sequences.</summary>
	/// <remarks>The comparison is performed over the raw UTF-16 memory of the strings, so no intermediate copies of the values are created.</remarks>
	/// <param name="left">The left value.</param>
	/// <param name="right">The right value.</param>
	/// <returns><c>true</c> if left and right are equal; <c>false</c> otherwise.</returns>
	public static bool FixedTimeEquals(string? left, string? right)
	{
		if (left is null || right is null) return left == right;
		return CryptographicOperations.FixedTimeEquals(MemoryMarshal.AsBytes(left.AsSpan()), MemoryMarshal.AsBytes(right.AsSpan()));
	}
}
