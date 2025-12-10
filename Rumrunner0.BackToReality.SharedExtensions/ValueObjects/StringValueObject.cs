using Rumrunner0.BackToReality.SharedExtensions.Exceptions;

namespace Rumrunner0.BackToReality.SharedExtensions.ValueObjects;

/// <summary>String value object.</summary>
public abstract record class StringValueObject
{
	/// <summary>Value.</summary>
	protected readonly string value;

	/// <inheritdoc cref="StringValueObject" />
	protected StringValueObject(string value)
	{
		ArgumentExceptionExtensions.ThrowIfNullOrEmptyOrWhiteSpace(value);
		this.value = value;
	}

	/// <inheritdoc />
	public override string ToString() => this.value;
}