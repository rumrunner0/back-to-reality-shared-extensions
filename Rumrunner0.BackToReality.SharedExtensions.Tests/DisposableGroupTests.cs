using System;
using System.Collections.Generic;
using Rumrunner0.BackToReality.SharedExtensions.Disposing;
using Xunit;

namespace Rumrunner0.BackToReality.SharedExtensions.Tests;

public sealed class DisposableGroupTests
{
	private sealed class TrackedDisposable : IDisposable
	{
		private readonly Action _onDispose;
		public TrackedDisposable(Action onDispose) => this._onDispose = onDispose;
		public void Dispose() => this._onDispose.Invoke();
	}

	[Fact]
	public void Constructor_ValidatesItems()
	{
		Assert.Throws<ArgumentNullException>(() => new DisposableGroup(null!));
		Assert.Throws<ArgumentException>(() => new DisposableGroup(new List<IDisposable>()));

		var withNull = new List<IDisposable> { new TrackedDisposable(() => { }), null! };
		var exception = Assert.Throws<ArgumentException>(() => new DisposableGroup(withNull));
		Assert.Equal("items", exception.ParamName);
	}

	[Fact]
	public void Dispose_RunsInReverseOrderExactlyOnce()
	{
		var order = new List<int>();
		var group = new DisposableGroup(new List<IDisposable>
		{
			new TrackedDisposable(() => order.Add(1)),
			new TrackedDisposable(() => order.Add(2)),
			new TrackedDisposable(() => order.Add(3)),
		});

		group.Dispose();
		group.Dispose();

		Assert.Equal(new[] { 3, 2, 1 }, order);
	}

	[Fact]
	public void Dispose_CollectsExceptionsAndKeepsDisposingEverything()
	{
		var order = new List<int>();
		var group = new DisposableGroup(new List<IDisposable>
		{
			new TrackedDisposable(() => order.Add(1)),
			new TrackedDisposable(() => throw new InvalidOperationException("boom")),
			new TrackedDisposable(() => order.Add(3)),
		});

		group.Dispose();

		Assert.Equal(new[] { 3, 1 }, order);
		var failure = Assert.Single(group.DisposalExceptions);
		Assert.IsType<InvalidOperationException>(failure);
	}

	[Fact]
	public void Members_ThrowAfterDisposal()
	{
		var group = new DisposableGroup(new List<IDisposable> { new TrackedDisposable(() => { }) });
		Assert.Single(group);
		Assert.NotNull(group[0]);

		group.Dispose();

		Assert.Throws<ObjectDisposedException>(() => group.Count);
		Assert.Throws<ObjectDisposedException>(() => group[0]);
		Assert.Throws<ObjectDisposedException>(() => group.GetEnumerator());
	}
}
