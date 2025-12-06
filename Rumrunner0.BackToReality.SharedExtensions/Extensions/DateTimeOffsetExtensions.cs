using System;
using Rumrunner0.BackToReality.SharedExtensions.Exceptions;

namespace Rumrunner0.BackToReality.SharedExtensions.Extensions;

/// <summary>Extensions for <see cref="DateTimeOffset" />.</summary>
public static class DateTimeOffsetExtensions
{
	/// <summary>Truncates a <paramref name="source" /> <see cref="DateTimeOffset" /> to the nearest lower multiple of the <paramref name="resolution" />.</summary>
	/// <param name="source">The <see cref="DateTimeOffset" />.</param>
	/// <param name="resolution">The resolution in ticks.</param>
	/// <returns>A new instance of the <see cref="DateTimeOffset" />.</returns>
	/// <exception cref="ArgumentException">If the resolution is less than or equal to zero.</exception>
	public static DateTimeOffset Truncate(this DateTimeOffset source, long resolution)
	{
		if (resolution <= 0) ArgumentExceptionExtensions.Throw("Value must be greater than zero", resolution);
		return new (source.UtcTicks - source.UtcTicks % resolution, source.Offset);
	}
}