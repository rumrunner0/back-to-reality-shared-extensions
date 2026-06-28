using System;
using System.Collections.Generic;
using Rumrunner0.BackToReality.SharedExtensions.Collections;
using Xunit;

namespace Rumrunner0.BackToReality.SharedExtensions.Tests;

public sealed class ListExtensionsTests
{
	[Fact]
	public void Add_AppendsAllItems()
	{
		var list = new List<int> { 1 };
		list.Add(new[] { 2, 3 });
		Assert.Equal(new[] { 1, 2, 3 }, list);
	}

	[Fact]
	public void AddNonNulls_SkipsNullStructsAndClasses()
	{
		var structs = new List<int>();
		structs.AddNonNulls(1, null, 3);
		Assert.Equal(new[] { 1, 3 }, structs);

		var classes = new List<string>();
		classes.AddNonNulls("a", null, "c");
		Assert.Equal(new[] { "a", "c" }, classes);
	}

	[Fact]
	public void AddNonNulls_ThrowsForNullItems()
	{
		var list = new List<string>();
		Assert.Throws<ArgumentNullException>(() => list.AddNonNulls((IEnumerable<string?>)null!));
	}

	[Fact]
	public void RemoveFirst_And_RemoveLast_RemoveSingleItems()
	{
		var list = new List<int> { 1, 2, 3 };
		list.RemoveFirst();
		Assert.Equal(new[] { 2, 3 }, list);
		list.RemoveLast();
		Assert.Equal(new[] { 2 }, list);
	}

	[Fact]
	public void RemoveFirst_And_RemoveLast_ThrowOnEmptyList()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() => new List<int>().RemoveFirst());
		Assert.Throws<ArgumentOutOfRangeException>(() => new List<int>().RemoveLast());
	}

	[Fact]
	public void RemoveBetween_RemovesInclusiveRange()
	{
		var list = new List<int> { 1, 2, 3, 4, 5 };
		list.RemoveBetween(start: 1, end: 3);
		Assert.Equal(new[] { 1, 5 }, list);

		var all = new List<int> { 1, 2, 3 };
		all.RemoveBetween(start: 0, end: 2);
		Assert.Empty(all);
	}

	[Fact]
	public void RemoveBetween_ThrowsOnInvalidRanges()
	{
		var list = new List<int> { 1, 2, 3, 4, 5 };
		Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveBetween(start: -1, end: 2));
		Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveBetween(start: 2, end: 1));
		Assert.Throws<ArgumentException>(() => list.RemoveBetween(start: 3, end: 7));
	}

	[Fact]
	public void RemoveFromStart_RemovesPrefixAndValidates()
	{
		var list = new List<int> { 1, 2, 3, 4 };
		list.RemoveFromStart(end: 1);
		Assert.Equal(new[] { 3, 4 }, list);

		Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveFromStart(end: -1));
	}

	[Fact]
	public void RemoveToEnd_RemovesSuffixAndValidates()
	{
		var list = new List<int> { 1, 2, 3, 4 };
		list.RemoveToEnd(start: 2);
		Assert.Equal(new[] { 1, 2 }, list);

		Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveToEnd(start: list.Count));
		Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveToEnd(start: -1));
		Assert.Throws<ArgumentOutOfRangeException>(() => new List<int>().RemoveToEnd(start: 0));
	}
}
