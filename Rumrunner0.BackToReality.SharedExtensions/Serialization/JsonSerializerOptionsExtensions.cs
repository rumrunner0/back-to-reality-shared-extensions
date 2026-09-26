using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Rumrunner0.BackToReality.SharedExtensions.Serialization;

/// <summary>Extensions for <see cref="JsonSerializerOptions" />.</summary>
public static class JsonSerializerOptionsExtensions
{
	private static readonly Lazy<JsonSerializerOptions> _betterWeb = new (() =>
	{
		var options = new JsonSerializerOptions().ConfigureBetterWeb();
		options.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
		options.MakeReadOnly();
		return options;
	});

	/// <summary>Gets a shared read-only <see cref="JsonSerializerOptions" /> instance preconfigured with the default settings.</summary>
	/// <returns>The shared <see cref="JsonSerializerOptions" /> instance.</returns>
	public static JsonSerializerOptions BetterWeb => _betterWeb.Value;

	/// <summary>Applies the default behavior.</summary>
	/// <param name="options">The options.</param>
	/// <returns>The same <paramref name="options" /> with applied defaults.</returns>
	public static JsonSerializerOptions ConfigureBetterWeb(this JsonSerializerOptions options)
	{
		options.PropertyNameCaseInsensitive = true;
		options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
		options.NumberHandling = JsonNumberHandling.Strict;

		options.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
		options.IgnoreReadOnlyProperties = false;

		// Declarations are enforced on the wire in both directions.
		// An explicit null in a non-nullable member throws on read and on write,
		// and a constructor parameter without a default value must be present.
		// Optional means "has a default value"; nullability alone does not make a parameter optional.
		options.RespectNullableAnnotations = true;
		options.RespectRequiredConstructorParameters = true;

		options.WriteIndented = true;
		options.IndentCharacter = '\t';
		options.IndentSize = 1;

		options.NewLine = "\n";
		options.AllowTrailingCommas = false;

		// If we ever need to extend the symbol range, here's the solution — but future us should perform extra research.
		// Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);

		return options;
	}
}