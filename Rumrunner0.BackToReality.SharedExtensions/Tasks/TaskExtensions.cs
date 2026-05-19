using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Rumrunner0.BackToReality.SharedExtensions.Tasks;

/// <summary>Task extensions.</summary>
public static class TaskExtensions
{
	/// <summary>
	/// Configures the awaiter for <paramref name="source"/> so the continuation does not marshal back
	/// to the captured <see cref="SynchronizationContext"/> or <see cref="TaskScheduler"/>, resuming wherever the task happens to complete. <br />
	/// Equivalent to <see cref="Task.ConfigureAwait(bool)" /> with <c>false</c>.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <returns>A new <see cref="ConfiguredTaskAwaitable" />.</returns>
	public static ConfiguredTaskAwaitable ContinueWithoutContextCapture(this Task source)
	{
		return source.ConfigureAwait(continueOnCapturedContext: false);
	}
}