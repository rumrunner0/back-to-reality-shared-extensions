using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Rumrunner0.BackToReality.SharedExtensions.Collections;
using Xunit;

namespace Rumrunner0.BackToReality.SharedExtensions.Tests;

public sealed class CollectionExtensionsTests
{
	/// <summary>Collection whose enumerators iterate a snapshot, so mutation during cycling is observable without versioning exceptions.</summary>
	private sealed class SnapshotCollection<T> : ICollection<T>
	{
		private readonly List<T> _items = new ();

		public int Count => this._items.Count;
		public bool IsReadOnly => false;
		public void Add(T item) => this._items.Add(item);
		public void Clear() => this._items.Clear();
		public bool Contains(T item) => this._items.Contains(item);
		public void CopyTo(T[] array, int arrayIndex) => this._items.CopyTo(array, arrayIndex);
		public bool Remove(T item) => this._items.Remove(item);
		public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)this._items.ToArray()).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
	}

	[Fact]
	public void Add_AppendsItemsToCollection()
	{
		ICollection<int> collection = new List<int> { 1 };
		collection.Add(new[] { 2, 3 });
		Assert.Equal(new[] { 1, 2, 3 }, collection);
	}

	[Fact]
	public void Add_ThrowsForNullItems()
	{
		ICollection<int> collection = new List<int>();
		Assert.Throws<ArgumentNullException>(() => collection.Add((IEnumerable<int>)null!));
	}

	[Fact]
	public void AddAndReturn_ReturnsTheItem()
	{
		ICollection<string> collection = new List<string>();
		Assert.Equal("x", collection.AddAndReturn("x"));
		Assert.Contains("x", collection);
	}

	[Fact]
	public void AddAndReturnCollection_ReturnsTheCollection()
	{
		ICollection<string> collection = new List<string>();
		Assert.Same(collection, collection.AddAndReturnCollection("x"));
		Assert.Contains("x", collection);
	}

	[Fact]
	public void Cycle_RepeatsItemsUntilCancelled()
	{
		using var cts = new CancellationTokenSource();
		ICollection<int> collection = new List<int> { 1, 2 };
		var taken = collection.Cycle(cts.Token).Take(5).ToArray();
		Assert.Equal(new[] { 1, 2, 1, 2, 1 }, taken);
	}

	[Fact]
	public void Cycle_EndsImmediatelyForEmptySourceOrCancelledToken()
	{
		ICollection<int> empty = new List<int>();
		Assert.Empty(empty.Cycle(CancellationToken.None));

		using var cts = new CancellationTokenSource();
		cts.Cancel();
		ICollection<int> collection = new List<int> { 1 };
		Assert.Empty(collection.Cycle(cts.Token));
	}

	[Fact]
	public void Cycle_EndsWhenSourceBecomesEmptyMidCycle()
	{
		var collection = new SnapshotCollection<int> { 1, 2, 3 };
		using var enumerator = collection.Cycle(CancellationToken.None).GetEnumerator();

		for (var i = 0; i < 3; i++)
		{
			Assert.True(enumerator.MoveNext());
		}

		collection.Clear();
		Assert.False(enumerator.MoveNext());
	}
}
