using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Rumrunner0.BackToReality.SharedExtensions.Exceptions;

namespace Rumrunner0.BackToReality.SharedExtensions.Disposing;

/// <summary>Disposable group.</summary>
public sealed class DisposableGroup : IReadOnlyList<IDisposable>, IDisposable
{
	private IDisposable[]? _items;

	/// <inheritdoc cref="DisposableGroup" />
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="items" /> is <c>null</c>.</exception>
	/// <exception cref="ArgumentException">Thrown if <paramref name="items" /> is empty or contains a <c>null</c> item.</exception>
	public DisposableGroup(IReadOnlyCollection<IDisposable> items)
	{
		ArgumentExceptionExtensions.ThrowIfNullOrEmpty(items);
		ArgumentExceptionExtensions.ThrowIfAnyNull(items);
		this._items = items.ToArray();
	}

	/// <summary>Exceptions thrown by the items during the last <see cref="Dispose" />.</summary>
	/// <value>Empty if none (or not yet disposed).</value>
	public IReadOnlyList<Exception> DisposalExceptions { get; private set; } = [];

	/// <inheritdoc />
	/// <remarks>
	/// Every item is disposed even if some throw, in reverse order to mirror nested <c>using</c> blocks.
	/// Failures are collected into <see cref="DisposalExceptions" />.
	/// </remarks>
	public void Dispose()
	{
		// Atomically grab the items and mark the group as disposed by setting items to null.
		var items = Interlocked.Exchange(ref this._items, value: null);
		if (items is null) return;

		// Dispose in reverse order to mimic nested "using" blocks.
		var exceptions = default(List<Exception>);
		for (var i = items.Length - 1; i >= 0; i--)
		{
			try
			{
				items[i]?.Dispose();
			}
			catch (Exception e)
			{
				exceptions ??= [];
				exceptions.Add(e);
			}
		}

		if (exceptions is not null) this.DisposalExceptions = exceptions;
	}

	/// <inheritdoc />
	/// <exception cref="ObjectDisposedException">Thrown if the group is disposed.</exception>
	public int Count => this.GetRequiredItems().Length;

	/// <inheritdoc />
	/// <exception cref="ObjectDisposedException">Thrown if the group is disposed.</exception>
	/// <exception cref="IndexOutOfRangeException">Thrown if <paramref name="index" /> is out of range.</exception>
	public IDisposable this[int index] => this.GetRequiredItems()[index];

	/// <inheritdoc />
	/// <exception cref="ObjectDisposedException">Thrown if the group is disposed.</exception>
	public IEnumerator<IDisposable> GetEnumerator() => ((IEnumerable<IDisposable>)this.GetRequiredItems()).GetEnumerator();

	/// <inheritdoc />
	IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

	/// <summary>Gets required items.</summary>
	/// <returns>The items.</returns>
	/// <exception cref="ObjectDisposedException">Thrown if items are disposed.</exception>
	private IDisposable[] GetRequiredItems() => this._items ?? throw new ObjectDisposedException(nameof(DisposableGroup));
}