using Rumrunner0.BackToReality.SharedExtensions.Exceptions;

namespace Rumrunner0.BackToReality.SharedExtensions.ValueObjects;

/// <summary>Value object.</summary>
public abstract record class ValueObject<T> where T : struct
{
	/// <summary>Value.</summary>
	protected readonly T value;

	/// <inheritdoc cref="ValueObject{T}" />
	protected ValueObject(T value)
	{
		ArgumentExceptionExtensions.ThrowIfNull(value);
		this.value = value;
	}

	/// <inheritdoc />
	public override string? ToString() => this.value.ToString();
}