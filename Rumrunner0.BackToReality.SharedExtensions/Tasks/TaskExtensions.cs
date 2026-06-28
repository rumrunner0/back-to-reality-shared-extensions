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

	/// <summary>
	/// Configures the awaiter for <paramref name="source"/> so the continuation does not marshal back
	/// to the captured <see cref="SynchronizationContext"/> or <see cref="TaskScheduler"/>, resuming wherever the task happens to complete. <br />
	/// Equivalent to <see cref="Task{TResult}.ConfigureAwait(bool)" /> with <c>false</c>.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <typeparam name="TResult">Type of the task result.</typeparam>
	/// <returns>A new <see cref="ConfiguredTaskAwaitable{TResult}" />.</returns>
	public static ConfiguredTaskAwaitable<TResult> ContinueWithoutContextCapture<TResult>(this Task<TResult> source)
	{
		return source.ConfigureAwait(continueOnCapturedContext: false);
	}

	/// <summary>
	/// Configures the awaiter for <paramref name="source"/> so the continuation does not marshal back
	/// to the captured <see cref="SynchronizationContext"/> or <see cref="TaskScheduler"/>, resuming wherever the task happens to complete. <br />
	/// Equivalent to <see cref="ValueTask.ConfigureAwait(bool)" /> with <c>false</c>.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <returns>A new <see cref="ConfiguredValueTaskAwaitable" />.</returns>
	public static ConfiguredValueTaskAwaitable ContinueWithoutContextCapture(this ValueTask source)
	{
		return source.ConfigureAwait(continueOnCapturedContext: false);
	}

	/// <summary>
	/// Configures the awaiter for <paramref name="source"/> so the continuation does not marshal back
	/// to the captured <see cref="SynchronizationContext"/> or <see cref="TaskScheduler"/>, resuming wherever the task happens to complete. <br />
	/// Equivalent to <see cref="ValueTask{TResult}.ConfigureAwait(bool)" /> with <c>false</c>.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <typeparam name="TResult">Type of the task result.</typeparam>
	/// <returns>A new <see cref="ConfiguredValueTaskAwaitable{TResult}" />.</returns>
	public static ConfiguredValueTaskAwaitable<TResult> ContinueWithoutContextCapture<TResult>(this ValueTask<TResult> source)
	{
		return source.ConfigureAwait(continueOnCapturedContext: false);
	}
}
