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

	/// <inheritdoc cref="_value" />
	public string Value => this._value;

	/// <inheritdoc />
	public sealed override string ToString() => this._value;
}