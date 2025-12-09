using System;

namespace Rumrunner0.BackToReality.SharedExtensions.Time;

/// <summary>Truncated time provider.</summary>
public sealed class TruncatedTimeProvider : TimeProvider
{
	/// <summary>Gets the current UTC time, truncated to <see cref="TimeSpan.TicksPerMicrosecond" />.</summary>
	public override DateTimeOffset GetUtcNow()
	{
		return base.GetUtcNow().Truncate(resolution: TimeSpan.TicksPerMicrosecond);
	}
}