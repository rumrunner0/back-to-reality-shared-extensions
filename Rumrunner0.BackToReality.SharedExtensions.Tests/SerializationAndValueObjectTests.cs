using System.Text.Json;
using Rumrunner0.BackToReality.SharedExtensions.Extensions;
using Rumrunner0.BackToReality.SharedExtensions.Serialization;
using Rumrunner0.BackToReality.SharedExtensions.ValueObjects;
using Xunit;

namespace Rumrunner0.BackToReality.SharedExtensions.Tests;

public sealed class SerializationAndValueObjectTests
{
	private readonly record struct UserId(int Value) : IValueObject<UserId, int>
	{
		public static UserId From(int value) => new (value);
	}

	private sealed record Sample(string SomeValue);

	[Fact]
	public void BetterWeb_IsASharedReadOnlyInstance()
	{
		Assert.Same(JsonSerializerOptionsExtensions.BetterWeb, JsonSerializerOptionsExtensions.BetterWeb);
		Assert.True(JsonSerializerOptionsExtensions.BetterWeb.IsReadOnly);
	}

	[Fact]
	public void BetterWeb_UsesCamelCaseAndTabIndentation()
	{
		var json = JsonSerializer.Serialize(new Sample("data"), JsonSerializerOptionsExtensions.BetterWeb);
		Assert.Contains("\"someValue\"", json);
		Assert.Contains("\t", json);
		Assert.DoesNotContain("\r\n", json);

		var parsed = JsonSerializer.Deserialize<Sample>("""{ "SOMEVALUE": "data" }""", JsonSerializerOptionsExtensions.BetterWeb);
		Assert.Equal("data", parsed?.SomeValue);
	}

	[Fact]
	public void ConfigureBetterWeb_AppliesDefaultsToFreshOptions()
	{
		var options = new JsonSerializerOptions().ConfigureBetterWeb();
		Assert.True(options.PropertyNameCaseInsensitive);
		Assert.Equal(JsonNamingPolicy.CamelCase, options.PropertyNamingPolicy);
		Assert.True(options.WriteIndented);
	}

	[Fact]
	public void ValueObject_SupportsCreationAndEquality()
	{
		var left = UserId.From(7);
		var right = UserId.From(7);
		Assert.Equal(7, left.Value);
		Assert.True(left.Equals(right));
		Assert.NotEqual(left, UserId.From(8));
	}

	[Fact]
	public void PragmaticEmailRegex_MatchesRealisticAddresses()
	{
		var regex = EmailAddressExtensions.PragmaticRegex();
		Assert.Matches(regex, "user@example.com");
		Assert.Matches(regex, "USER.NAME+tag@EXAMPLE.CO.UK");
		Assert.DoesNotMatch(regex, "not-an-email");
		Assert.DoesNotMatch(regex, "missing@dot");
		Assert.DoesNotMatch(regex, "two@@example.com");
	}
}
