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
	public DisposableGroup(IList<IDisposable> items)
	{
		ArgumentExceptionExtensions.ThrowIfNullOrEmpty(items);
		ArgumentExceptionExtensions.ThrowIfAnyNull(items);
		this._items = items.ToArray();
	}

	/// <inheritdoc />
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

		if (exceptions is not null)
		{
			throw new AggregateException("One or more disposables failed to dispose", exceptions);
		}
	}

	/// <inheritdoc />
	public int Count => this.GetRequiredItems().Length;

	/// <inheritdoc />
	public IDisposable this[int index] => this.GetRequiredItems()[index];

	/// <inheritdoc />
	public IEnumerator<IDisposable> GetEnumerator() => ((IEnumerable<IDisposable>)this.GetRequiredItems()).GetEnumerator();

	/// <inheritdoc />
	IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

	/// <summary>Gets required items.</summary>
	/// <returns>The items.</returns>
	/// <exception cref="ObjectDisposedException">Thrown if items are disposed.</exception>
	private IDisposable[] GetRequiredItems() => this._items ?? throw new ObjectDisposedException(nameof(DisposableGroup));
}