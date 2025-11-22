using System;
using Rumrunner0.BackToReality.SharedExtensions.Exceptions;
using Rumrunner0.BackToReality.SharedExtensions.Extensions;

namespace Rumrunner0.BackToReality.SharedExtensions.Environment;

/// <summary>Extensions for environment variables.</summary>
public static class EnvironmentVariableExtensions
{
	/// <summary>Retrieves the value of an environment variable that must be set.</summary>
	/// <param name="name">The name.</param>
	/// <returns>The value.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the variable is not set.</exception>
	/// <remarks>See also <see cref="System.Environment.GetEnvironmentVariable(string)" />.</remarks>
	public static string GetRequired(string name)
	{
		ArgumentExceptionExtensions.ThrowIfNullOrEmptyOrWhiteSpace(name);

		var variable = System.Environment.GetEnvironmentVariable(name);
		if (variable is null) throw new InvalidOperationException($"'{name}' environment variable is not set");
		if (variable.IsEmptyOrWhitespace()) throw new InvalidOperationException($"'{name}' environment variable value is empty");
		return variable;
	}

	/// <inheritdoc cref="GetRequired(string)" />
	/// <exception cref="InvalidOperationException">Thrown if the variable is not valid.</exception>
	public static int GetRequiredInt(string name)
	{
		var variable = GetRequired(name);
		return int.TryParse(variable, out var i) ? i : throw new InvalidOperationException($"'{name}' is not valid (current value: '{variable}')");
	}
}