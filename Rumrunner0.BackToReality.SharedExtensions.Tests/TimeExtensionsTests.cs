using System;
using Rumrunner0.BackToReality.SharedExtensions.Time;
using Xunit;

namespace Rumrunner0.BackToReality.SharedExtensions.Tests;

public sealed class TimeExtensionsTests
{
	[Fact]
	public void Truncate_TruncatesToTheResolution()
	{
		var source = new DateTimeOffset(2026, 7, 18, 14, 35, 42, TimeSpan.FromHours(2)).AddTicks(1_234_567);
		var truncated = source.Truncate(TimeSpan.TicksPerHour);

		Assert.Equal(new DateTimeOffset(2026, 7, 18, 14, 0, 0, TimeSpan.FromHours(2)), truncated);
		Assert.Equal(source.Offset, truncated.Offset);
	}

	[Fact]
	public void Truncate_KeepsAlreadyAlignedValues()
	{
		var aligned = new DateTimeOffset(2026, 7, 18, 14, 0, 0, TimeSpan.Zero);
		Assert.Equal(aligned, aligned.Truncate(TimeSpan.TicksPerHour));
	}

	[Fact]
	public void Truncate_ValidatesTheResolution()
	{
		var source = new DateTimeOffset(2026, 7, 18, 12, 0, 0, TimeSpan.Zero);
		Assert.Throws<ArgumentException>(() => source.Truncate(0));
		Assert.Throws<ArgumentException>(() => source.Truncate(-1));
	}

	[Fact]
	public void Truncate_ThrowsWhenTheResultFallsBelowTheUtcMinimum()
	{
		var nearMinimum = new DateTimeOffset(1, 1, 1, 2, 0, 0, TimeSpan.FromHours(2));
		Assert.Throws<ArgumentOutOfRangeException>(() => nearMinimum.Truncate(TimeSpan.TicksPerDay));
	}

	[Fact]
	public void TruncatedTimeProvider_TruncatesToMicroseconds()
	{
		var provider = new TruncatedTimeProvider();
		var now = provider.GetUtcNow();
		Assert.Equal(0, now.Ticks % TimeSpan.TicksPerMicrosecond);
		Assert.Equal(TimeSpan.Zero, now.Offset);
	}
}
