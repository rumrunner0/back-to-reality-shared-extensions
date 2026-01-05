namespace Rumrunner0.BackToReality.SharedExtensions.ValueObjects;

/// <summary>Value object.</summary>
/// <typeparam name="TSelf">The self type.</typeparam>
/// <typeparam name="TValue">The value type.</typeparam>
public interface IValueObject<out TSelf, TValue> where TSelf : struct, IValueObject<TSelf, TValue> where TValue : notnull
{
	/// <summary>Value.</summary>
	TValue Value { get; }

	/// <summary>Creates an <see cref="IValueObject{TSelf,TValue}" /> from <paramref name="value" />.</summary>
	/// <param name="value">The value.</param>
	/// <returns>A new <see cref="IValueObject{TSelf,TValue}" />.</returns>
	static abstract TSelf From(TValue value);
}