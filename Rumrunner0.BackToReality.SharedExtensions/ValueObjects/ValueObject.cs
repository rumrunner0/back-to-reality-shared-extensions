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

	/// <summary>Value.</summary>
	public T Value => this._value;

	/// <inheritdoc />
	public sealed override string? ToString() => this._value.ToString();

	/// <summary>Implicitly converts a <see cref="ValueObject{T}" /> to a <see cref="string" />.</summary>
	/// <param name="source">The <see cref="ValueObject{T}" />.</param>
	/// <remarks>The result <see cref="string" /> can be <c>null</c>.</remarks>
	public static implicit operator string?(ValueObject<T> source) => source.ToString();
}