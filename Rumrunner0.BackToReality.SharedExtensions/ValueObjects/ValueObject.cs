using Rumrunner0.BackToReality.SharedExtensions.Exceptions;

namespace Rumrunner0.BackToReality.SharedExtensions.ValueObjects;

/// <summary>Value object.</summary>
public abstract record class ValueObject<T> where T : struct
{
	/// <summary>Value.</summary>
	private readonly T _value;

	/// <inheritdoc cref="ValueObject{T}" />
	protected ValueObject(T value)
	{
		ArgumentExceptionExtensions.ThrowIfNull(value);
		this._value = value;
	}

	/// <inheritdoc cref="_value" />
	public T Value => this._value;

	/// <inheritdoc />
	public sealed override string? ToString() => this._value.ToString();
}