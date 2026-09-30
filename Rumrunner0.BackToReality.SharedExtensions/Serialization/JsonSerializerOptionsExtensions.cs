using System;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Rumrunner0.BackToReality.SharedExtensions.Collections;

namespace Rumrunner0.BackToReality.SharedExtensions.Serialization;

/// <summary>Extensions for <see cref="JsonSerializerOptions" />.</summary>
public static class JsonSerializerOptionsExtensions
{
	private static readonly Lazy<JsonSerializerOptions> _betterWeb = new (() =>
	{
		var options = new JsonSerializerOptions().ConfigureBetterWeb();
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

		// Collection members marked with JsonIgnoreWhenEmptyAttribute are omitted when empty,
		// so absence and emptiness are one state on the wire.
		options.TypeInfoResolver = (options.TypeInfoResolver ?? new DefaultJsonTypeInfoResolver()).WithAddedModifier(OmitEmptyCollections);

		return options;
	}

	/// <summary>Omits the members marked with <see cref="JsonIgnoreWhenEmptyAttribute" /> from the JSON when they are empty.</summary>
	/// <param name="typeInfo">The type info being resolved.</param>
	/// <exception cref="InvalidOperationException">Thrown when a marked member is not a non-nullable collection.</exception>
	private static void OmitEmptyCollections(JsonTypeInfo typeInfo)
	{
		// Skips type if it's not an object.
		if (typeInfo.Kind is not JsonTypeInfoKind.Object)
		{
			return;
		}

		foreach (var property in typeInfo.Properties)
		{
			// Skips the member without attribute.
			if (property.AttributeProvider?.IsDefined(typeof(JsonIgnoreWhenEmptyAttribute), inherit: false) != true)
			{
				continue;
			}

			// Validates the member.
			if (property.PropertyType == typeof(string) || !typeof(IEnumerable).IsAssignableFrom(property.PropertyType) || property.IsGetNullable)
			{
				throw new InvalidOperationException($"The member '{property.Name}' of '{typeInfo.Type}' is marked with {nameof(JsonIgnoreWhenEmptyAttribute)} but is not a non-nullable collection");
			}

			// Decides whether the member should be serialized or not.
			// The predicate returns true for a null value on purpose.
			// The null then reaches the nullability check, which throws because a marked member is never nullable.
			// Returning false instead would omit the member and hide the null.
			property.ShouldSerialize = static (_, value) =>
			{
				return value is null || ((IEnumerable)value).Some();
			};
		}
	}
}