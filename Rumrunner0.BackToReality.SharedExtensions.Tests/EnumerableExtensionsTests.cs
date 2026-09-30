using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Rumrunner0.BackToReality.SharedExtensions.Collections;
using Xunit;

namespace Rumrunner0.BackToReality.SharedExtensions.Tests;

public sealed class EnumerableExtensionsTests
{
	[SuppressMessage("Sonar", "S2190", Justification = "The endless sequence is the point — the helpers under test must terminate without exhausting it.")]
	private static IEnumerable<int> Infinite()
	{
		while (true) yield return 1;
	}

	/// <summary>Wraps the items in a non-generic sequence without a count, so the enumerating path is exercised.</summary>
	private static IEnumerable Lazy(int[] items)
	{
		foreach (var item in items) yield return item;
	}

	/// <summary>A non-generic collection that reports a count without holding the items.</summary>
	private sealed class CountedOnly(int count) : ICollection
	{
		public int Count => count;
		public bool IsSynchronized => false;
		public object SyncRoot => this;
		public void CopyTo(Array array, int index) => throw new NotSupportedException();
		public IEnumerator GetEnumerator() => throw new NotSupportedException();
	}

	[Fact]
	public void StringJoin_JoinsWithSeparator()
	{
		Assert.Equal("1-2-3", new[] { 1, 2, 3 }.StringJoin("-"));
		Assert.Equal("1 2 3", new[] { 1, 2, 3 }.StringJoin());
		Assert.Equal(string.Empty, Array.Empty<int>().StringJoin("-"));
	}

	[Fact]
	public void IsNullOrEmpty_And_IsNotNullAndNotEmpty_CoverAllCases()
	{
		Assert.True(((IEnumerable<int>?)null).IsNullOrEmpty());
		Assert.True(Array.Empty<int>().IsNullOrEmpty());
		Assert.False(new[] { 1 }.IsNullOrEmpty());

		Assert.False(((IEnumerable<int>?)null).IsNotNullAndNotEmpty());
		Assert.False(Array.Empty<int>().IsNotNullAndNotEmpty());
		Assert.True(new[] { 1 }.IsNotNullAndNotEmpty());
	}

	[Fact]
	public void None_Some_Many_MatchItemCounts()
	{
		Assert.True(Array.Empty<int>().None());
		Assert.False(new[] { 1 }.None());

		Assert.False(Array.Empty<int>().Some());
		Assert.True(new[] { 1 }.Some());

		Assert.False(new[] { 1 }.Many());
		Assert.True(new[] { 1, 2 }.Many());

		Assert.Throws<ArgumentNullException>(() => ((IEnumerable<int>)null!).None());
		Assert.Throws<ArgumentNullException>(() => ((IEnumerable<int>)null!).Some());
		Assert.Throws<ArgumentNullException>(() => ((IEnumerable<int>)null!).Many());
	}

	[Fact]
	public void NonGeneric_IsNullOrEmpty_And_IsNotNullAndNotEmpty_CoverAllCases()
	{
		Assert.True(((IEnumerable?)null).IsNullOrEmpty());
		Assert.True(((IEnumerable)Array.Empty<int>()).IsNullOrEmpty());
		Assert.False(((IEnumerable)new[] { 1 }).IsNullOrEmpty());

		Assert.False(((IEnumerable?)null).IsNotNullAndNotEmpty());
		Assert.False(((IEnumerable)Array.Empty<int>()).IsNotNullAndNotEmpty());
		Assert.True(((IEnumerable)new[] { 1 }).IsNotNullAndNotEmpty());
	}

	[Fact]
	public void NonGeneric_None_Some_Many_MatchItemCounts()
	{
		Assert.True(((IEnumerable)Array.Empty<int>()).None());
		Assert.False(((IEnumerable)new[] { 1 }).None());

		Assert.False(((IEnumerable)Array.Empty<int>()).Some());
		Assert.True(((IEnumerable)new[] { 1 }).Some());

		Assert.False(((IEnumerable)new[] { 1 }).Many());
		Assert.True(((IEnumerable)new[] { 1, 2 }).Many());

		// A hash set is a generic collection without the non-generic count, so it takes the enumerating path.
		Assert.True(((IEnumerable)new HashSet<int>()).None());
		Assert.True(((IEnumerable)new HashSet<int> { 1 }).Some());
		Assert.True(((IEnumerable)new HashSet<int> { 1, 2 }).Many());

		Assert.Throws<ArgumentNullException>(() => ((IEnumerable)null!).None());
		Assert.Throws<ArgumentNullException>(() => ((IEnumerable)null!).Some());
		Assert.Throws<ArgumentNullException>(() => ((IEnumerable)null!).Many());
	}

	[Theory]
	[InlineData(new int[0], 0, true)]
	[InlineData(new[] { 1, 2 }, 2, true)]
	[InlineData(new[] { 1, 2 }, 1, false)]
	[InlineData(new[] { 1, 2 }, 3, false)]
	[InlineData(new[] { 1, 2 }, -1, false)]
	public void NonGenericExactly_MatchesOnlyTheExactCount(int[] source, int count, bool expected)
	{
		Assert.Equal(expected, ((IEnumerable)source).Exactly(count));
		Assert.Equal(expected, Lazy(source).Exactly(count));
	}

	[Theory]
	[InlineData(new int[0], -1, true)]
	[InlineData(new int[0], 0, false)]
	[InlineData(new[] { 1, 2 }, 1, true)]
	[InlineData(new[] { 1, 2 }, 2, false)]
	public void NonGenericMoreThan_ComparesStrictly(int[] source, int count, bool expected)
	{
		Assert.Equal(expected, ((IEnumerable)source).MoreThan(count));
		Assert.Equal(expected, Lazy(source).MoreThan(count));
	}

	[Theory]
	[InlineData(new int[0], 0, false)]
	[InlineData(new[] { 1, 2 }, 3, true)]
	[InlineData(new[] { 1, 2 }, 2, false)]
	[InlineData(new[] { 1, 2 }, -1, false)]
	public void NonGenericLessThan_ComparesStrictly(int[] source, int count, bool expected)
	{
		Assert.Equal(expected, ((IEnumerable)source).LessThan(count));
		Assert.Equal(expected, Lazy(source).LessThan(count));
	}

	[Theory]
	[InlineData(new int[0], 0, true)]
	[InlineData(new[] { 1, 2 }, 2, true)]
	[InlineData(new[] { 1, 2 }, 3, false)]
	[InlineData(new[] { 1, 2 }, -5, true)]
	public void NonGenericAtLeast_ComparesInclusively(int[] source, int count, bool expected)
	{
		Assert.Equal(expected, ((IEnumerable)source).AtLeast(count));
		Assert.Equal(expected, Lazy(source).AtLeast(count));
	}

	[Fact]
	public void NonGenericExactly_DoesNotOverflowAtIntMaxValue()
	{
		Assert.True(new CountedOnly(int.MaxValue).Exactly(int.MaxValue));
		Assert.False(new CountedOnly(int.MaxValue - 1).Exactly(int.MaxValue));
		Assert.False(new CountedOnly(int.MaxValue).MoreThan(int.MaxValue));
		Assert.True(new CountedOnly(int.MaxValue).AtLeast(int.MaxValue));
	}

	[Fact]
	public void NonGenericCountingHelpers_TerminateOnInfiniteSequences()
	{
		var infinite = (IEnumerable)Infinite();

		Assert.True(infinite.AtLeast(3));
		Assert.True(infinite.MoreThan(5));
		Assert.True(infinite.Some());
		Assert.False(infinite.None());
		Assert.False(infinite.Exactly(2));
		Assert.False(infinite.LessThan(4));
	}

	[Theory]
	[InlineData(new int[0], 0, true)]
	[InlineData(new[] { 1, 2 }, 2, true)]
	[InlineData(new[] { 1, 2 }, 1, false)]
	[InlineData(new[] { 1, 2 }, 3, false)]
	[InlineData(new[] { 1, 2 }, -1, false)]
	public void Exactly_MatchesOnlyTheExactCount(int[] source, int count, bool expected)
	{
		Assert.Equal(expected, source.Exactly(count));
	}

	[Theory]
	[InlineData(new int[0], -1, true)]
	[InlineData(new int[0], 0, false)]
	[InlineData(new[] { 1, 2 }, 1, true)]
	[InlineData(new[] { 1, 2 }, 2, false)]
	public void MoreThan_ComparesStrictly(int[] source, int count, bool expected)
	{
		Assert.Equal(expected, source.MoreThan(count));
	}

	[Theory]
	[InlineData(new int[0], 0, false)]
	[InlineData(new[] { 1, 2 }, 3, true)]
	[InlineData(new[] { 1, 2 }, 2, false)]
	[InlineData(new[] { 1, 2 }, -1, false)]
	public void LessThan_ComparesStrictly(int[] source, int count, bool expected)
	{
		Assert.Equal(expected, source.LessThan(count));
	}

	[Theory]
	[InlineData(new int[0], 0, true)]
	[InlineData(new[] { 1, 2 }, 2, true)]
	[InlineData(new[] { 1, 2 }, 3, false)]
	[InlineData(new[] { 1, 2 }, -5, true)]
	public void AtLeast_ComparesInclusively(int[] source, int count, bool expected)
	{
		Assert.Equal(expected, source.AtLeast(count));
	}

	[Fact]
	public void Exactly_DoesNotOverflowAtIntMaxValue()
	{
		Assert.True(Enumerable.Range(0, int.MaxValue).Exactly(int.MaxValue));
		Assert.False(Enumerable.Range(0, int.MaxValue - 1).Exactly(int.MaxValue));
		Assert.False(Enumerable.Range(0, int.MaxValue).MoreThan(int.MaxValue));
		Assert.True(Enumerable.Range(0, int.MaxValue).AtLeast(int.MaxValue));
	}

	[Fact]
	public void CountingHelpers_TerminateOnInfiniteSequences()
	{
		Assert.True(Infinite().AtLeast(3));
		Assert.True(Infinite().MoreThan(5));
		Assert.True(Infinite().Some());
		Assert.False(Infinite().None());
		Assert.False(Infinite().Exactly(2));
		Assert.False(Infinite().LessThan(4));
	}
}
