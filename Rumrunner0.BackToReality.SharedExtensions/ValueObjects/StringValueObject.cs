using Rumrunner0.BackToReality.SharedExtensions.Exceptions;

namespace Rumrunner0.BackToReality.SharedExtensions.ValueObjects;

/// <summary>String value object.</summary>
public abstract record class StringValueObject
{
	/// <summary>Value.</summary>
	private readonly string _value;

	/// <inheritdoc cref="StringValueObject" />
	protected StringValueObject(string value)
	{
		ArgumentExceptionExtensions.ThrowIfNullOrEmptyOrWhiteSpace(value);
		this._value = value;
	}

	/// <summary>Value.</summary>
	public string Value => this._value;

	/// <inheritdoc />
	public sealed override string ToString() => this._value;

	/// <summary>Implicitly converts a <see cref="StringValueObject" /> to a <see cref="string" />.</summary>
	/// <param name="source">The <see cref="StringValueObject" />.</param>
	public static implicit operator string(StringValueObject source) => source.Value;
}