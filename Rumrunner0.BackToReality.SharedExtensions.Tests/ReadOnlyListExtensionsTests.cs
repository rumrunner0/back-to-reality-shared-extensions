using System;
using System.Collections.Generic;
using System.Linq;
using Rumrunner0.BackToReality.SharedExtensions.Collections;
using Xunit;

namespace Rumrunner0.BackToReality.SharedExtensions.Tests;

public sealed class ReadOnlyListExtensionsTests
{
	[Fact]
	public void First_And_Last_ReturnBoundaryItems()
	{
		var list = new List<int> { 10, 20, 30 };
		Assert.Equal(10, list.First());
		Assert.Equal(30, list.Last());
	}

	[Fact]
	public void First_And_Last_ThrowInvalidOperationOnEmpty_MatchingLinq()
	{
		// System.Linq is imported in this file, so this also proves the IReadOnlyList
		// overloads win overload resolution over Enumerable.First/Last for List<T>.
		var empty = new List<int>();
		var first = Assert.Throws<InvalidOperationException>(() => empty.First());
		var last = Assert.Throws<InvalidOperationException>(() => empty.Last());
		Assert.Contains("at least", first.Message);
		Assert.Contains("at least", last.Message);
	}

	[Fact]
	public void Deconstruct_ReturnsTuplesOfEachArity()
	{
		var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
		Assert.Equal((1, 2), list.Deconstruct2());
		Assert.Equal((1, 2, 3), list.Deconstruct3());
		Assert.Equal((1, 2, 3, 4), list.Deconstruct4());
		Assert.Equal((1, 2, 3, 4, 5), list.Deconstruct5());
		Assert.Equal((1, 2, 3, 4, 5, 6), list.Deconstruct6());
		Assert.Equal((1, 2, 3, 4, 5, 6, 7), list.Deconstruct7());
		Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8), list.Deconstruct8());
	}

	[Fact]
	public void Deconstruct_ThrowsInvalidOperationWhenListIsTooShort()
	{
		var list = new List<int> { 1 };
		Assert.Throws<InvalidOperationException>(() => list.Deconstruct2());
		Assert.Throws<InvalidOperationException>(() => list.Deconstruct8());
	}
}
