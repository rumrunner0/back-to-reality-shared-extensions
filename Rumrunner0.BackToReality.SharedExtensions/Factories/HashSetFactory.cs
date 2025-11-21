using System.Collections.Generic;

namespace Rumrunner0.BackToReality.SharedExtensions.Factories;

/// <summary>Factory for <see cref="HashSet{T}" />.</summary>
public static class HashSetFactory
{
	/// <summary>Creates a new <see cref="HashSet{T}" /> that uses <see cref="ReferenceEqualityComparer" />.</summary>
	/// <typeparam name="T">The type of items in the set.</typeparam>
	/// <returns>A new <see cref="HashSet{T}" /> that uses <see cref="ReferenceEqualityComparer" />.</returns>
	public static HashSet<T> ReferenceEquality<T>() where T : class
	{
		return new (ReferenceEqualityComparer.Instance);
	}

	/// <summary>Creates a new <see cref="HashSet{T}" /> that uses <see cref="ReferenceEqualityComparer" />.</summary>
	/// <param name="collection">The existing collection.</param>
	/// <typeparam name="T">The type of items in the set.</typeparam>
	/// <returns>A new <see cref="HashSet{T}" /> that uses <see cref="ReferenceEqualityComparer" />.</returns>
	public static HashSet<T> ReferenceEquality<T>(IEnumerable<T> collection) where T : class
	{
		return new (collection, ReferenceEqualityComparer.Instance);
	}
}