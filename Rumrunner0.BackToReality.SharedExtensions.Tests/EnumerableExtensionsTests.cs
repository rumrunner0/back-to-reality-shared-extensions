using System;
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
