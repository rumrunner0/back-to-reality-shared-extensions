using System;
using System.Collections.Generic;
using Rumrunner0.BackToReality.SharedExtensions.Collections;
using Xunit;

namespace Rumrunner0.BackToReality.SharedExtensions.Tests;

public sealed class ArrayAndHashSetTests
{
	[Fact]
	public void CreateFromNonNulls_FiltersNullsForStructsAndClasses()
	{
		Assert.Equal(new[] { 1, 3 }, ArrayExtensions.CreateFromNonNulls<int>(1, null, 3));
		Assert.Equal(new[] { "a", "c" }, ArrayExtensions.CreateFromNonNulls("a", null, "c"));
		Assert.Empty(ArrayExtensions.CreateFromNonNulls<int>());
	}

	[Fact]
	public void CreateFromNonNulls_ThrowsForNullItems()
	{
		Assert.Throws<ArgumentNullException>(() => ArrayExtensions.CreateFromNonNulls((IEnumerable<string?>)null!));
	}

	[Fact]
	public void ReferenceEquality_UsesReferenceSemantics()
	{
		var left = new string("value".ToCharArray());
		var right = new string("value".ToCharArray());
		Assert.Equal(left, right);
		Assert.NotSame(left, right);

		var set = HashSetFactory.ReferenceEquality<string>();
		Assert.True(set.Add(left));
		Assert.True(set.Add(right));
		Assert.False(set.Add(left));

		var prefilled = HashSetFactory.ReferenceEquality(new[] { left, left, right });
		Assert.Equal(2, prefilled.Count);
	}
}
