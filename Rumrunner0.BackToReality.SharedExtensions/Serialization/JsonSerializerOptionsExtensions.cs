using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rumrunner0.BackToReality.SharedExtensions.Serialization;

/// <summary>Extensions for <see cref="JsonSerializerOptions" />.</summary>
public static class JsonSerializerOptionsExtensions
{
	private static JsonSerializerOptions? _betterWeb;

	/// <summary>Creates a new <see cref="JsonSerializerOptions" /> preconfigured with the default settings.</summary>
	/// <returns>A new <see cref="JsonSerializerOptions" />.</returns>
	public static JsonSerializerOptions BetterWeb
	{
		get
		{
			if (_betterWeb is { } cached) return cached;
			_betterWeb = new JsonSerializerOptions().ConfigureBetterWeb();
			_betterWeb.MakeReadOnly();
			return _betterWeb;
		}
	}

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