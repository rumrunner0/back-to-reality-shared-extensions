using System.Security.Cryptography;
using System.Text;

namespace Rumrunner0.BackToReality.SharedExtensions.Cryptography;

/// <summary>Extensions for <see cref="CryptographicOperations" />.</summary>
public static class CryptographicOperationExtensions
{
	/// <summary>Determines the equality of two strings in an amount of time that depends on the length of the sequences.</summary>
	/// <param name="left">The left value.</param>
	/// <param name="right">The right value.</param>
	/// <returns><c>true</c>, if left and right are equal; <c>false</c>, otherwise.</returns>
	public static bool FixedTimeEquals(string? left, string? right)
	{
		if (left is null || right is null) return left == right;

		var leftBytes = Encoding.UTF8.GetBytes(left);
		var rightBytes = Encoding.UTF8.GetBytes(right);

		if (leftBytes.Length != rightBytes.Length) return false;

		return CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
	}
}