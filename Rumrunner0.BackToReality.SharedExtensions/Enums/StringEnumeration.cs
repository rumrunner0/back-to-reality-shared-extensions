using Rumrunner0.BackToReality.SharedExtensions.Exceptions;

namespace Rumrunner0.BackToReality.SharedExtensions.Enums;

// TODO: TSelf is needed for future features to make it a closed set.

/// <summary>String enumeration.</summary>
/// <typeparam name="TSelf">The self type.</typeparam>
public abstract record class StringEnumeration<TSelf> where TSelf : StringEnumeration<TSelf>
{
	/// <summary>Value.</summary>
	private readonly string _value;

	/// <inheritdoc cref="StringEnumeration{TSelf}" />
	protected StringEnumeration(string value)
	{
		ArgumentExceptionExtensions.ThrowIfNullOrEmptyOrWhiteSpace(value);
		this._value = value;
	}

	/// <summary>Value.</summary>
	public string Value => this._value;

	/// <inheritdoc />
	public sealed override string ToString() => this._value;

	/// <summary>Implicitly converts a <see cref="StringEnumeration{TSelf}" /> to a <see cref="string" />.</summary>
	/// <param name="source">The <see cref="StringEnumeration{TSelf}" />.</param>
	public static implicit operator string(StringEnumeration<TSelf> source) => source.Value;
}