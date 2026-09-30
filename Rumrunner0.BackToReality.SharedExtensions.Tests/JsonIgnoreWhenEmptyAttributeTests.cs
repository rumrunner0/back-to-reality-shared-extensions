using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Rumrunner0.BackToReality.SharedExtensions.Serialization;
using Xunit;

namespace Rumrunner0.BackToReality.SharedExtensions.Tests;

public sealed class JsonIgnoreWhenEmptyAttributeTests
{
	private sealed record class Envelope
	{
		public required string Content { get; init; }

		[JsonIgnoreWhenEmpty]
		public IReadOnlyList<string> Warnings { get; init; } = [];

		[JsonIgnoreWhenEmpty]
		public string[] Tags { get; init; } = [];

		[JsonIgnoreWhenEmpty]
		public IEnumerable<int> Numbers { get; init; } = [];
	}

	private sealed record class NullableMarked
	{
		[JsonIgnoreWhenEmpty]
		public IReadOnlyList<string>? Warnings { get; init; }
	}

	private sealed record class StringMarked
	{
		[JsonIgnoreWhenEmpty]
		public string Text { get; init; } = string.Empty;
	}

	private sealed record class ScalarMarked
	{
		[JsonIgnoreWhenEmpty]
		public int Count { get; init; }
	}

	[Fact]
	public void BetterWeb_OmitsMarkedCollectionsWhenEmpty()
	{
		var json = JsonSerializer.Serialize(new Envelope { Content = "c" }, JsonSerializerOptionsExtensions.BetterWeb);
		Assert.Equal("{\n\t\"content\": \"c\"\n}", json);
	}

	[Fact]
	public void BetterWeb_WritesMarkedCollectionsWhenNotEmpty()
	{
		var json = JsonSerializer.Serialize(new Envelope { Content = "c", Warnings = ["w"], Tags = ["t"], Numbers = new[] { 1, 2 }.Where(n => n > 1) }, JsonSerializerOptionsExtensions.BetterWeb);
		Assert.Equal("{\n\t\"content\": \"c\",\n\t\"warnings\": [\n\t\t\"w\"\n\t],\n\t\"tags\": [\n\t\t\"t\"\n\t],\n\t\"numbers\": [\n\t\t2\n\t]\n}", json);
	}

	[Fact]
	public void BetterWeb_OmitsALazySequenceThatYieldsNothing()
	{
		var json = JsonSerializer.Serialize(new Envelope { Content = "c", Numbers = new[] { 1 }.Where(n => n > 1) }, JsonSerializerOptionsExtensions.BetterWeb);
		Assert.DoesNotContain("numbers", json);
	}

	[Fact]
	public void BetterWeb_ReadsAnAbsentMarkedCollectionAsEmpty()
	{
		var parsed = JsonSerializer.Deserialize<Envelope>("""{ "content": "c" }""", JsonSerializerOptionsExtensions.BetterWeb);
		Assert.NotNull(parsed);
		Assert.Empty(parsed.Warnings);
		Assert.Empty(parsed.Tags);
		Assert.Empty(parsed.Numbers);
	}

	[Fact]
	public void BetterWeb_ReadsAPresentMarkedCollection()
	{
		var parsed = JsonSerializer.Deserialize<Envelope>("""{ "content": "c", "warnings": ["w"], "tags": [], "numbers": [3] }""", JsonSerializerOptionsExtensions.BetterWeb);
		Assert.NotNull(parsed);
		Assert.Equal(new[] { "w" }, parsed.Warnings);
		Assert.Empty(parsed.Tags);
		Assert.Equal(new[] { 3 }, parsed.Numbers);
	}

	[Fact]
	public void BetterWeb_RoundTripsAnEmptyMarkedCollectionThroughAbsence()
	{
		var json = JsonSerializer.Serialize(new Envelope { Content = "c" }, JsonSerializerOptionsExtensions.BetterWeb);
		var back = JsonSerializer.Deserialize<Envelope>(json, JsonSerializerOptionsExtensions.BetterWeb);
		Assert.NotNull(back);
		Assert.Empty(back.Warnings);
	}

	[Fact]
	public void BetterWeb_StillRejectsNullInAMarkedCollectionOnRead()
	{
		Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Envelope>("""{ "content": "c", "warnings": null }""", JsonSerializerOptionsExtensions.BetterWeb));
	}

	[Fact]
	public void BetterWeb_StillRejectsNullInAMarkedCollectionOnWrite()
	{
		Assert.Throws<JsonException>(() => JsonSerializer.Serialize(new Envelope { Content = "c", Warnings = null! }, JsonSerializerOptionsExtensions.BetterWeb));
	}

	[Theory]
	[InlineData(typeof(NullableMarked))]
	[InlineData(typeof(StringMarked))]
	[InlineData(typeof(ScalarMarked))]
	public void BetterWeb_RejectsTheAttributeOnAnythingButANonNullableCollection(Type type)
	{
		var exception = Assert.Throws<InvalidOperationException>(() => JsonSerializerOptionsExtensions.BetterWeb.GetTypeInfo(type));
		Assert.Contains(nameof(JsonIgnoreWhenEmptyAttribute), exception.Message);
	}

	[Fact]
	public void ConfigureBetterWeb_SetsAResolverOnFreshOptions()
	{
		var options = new JsonSerializerOptions().ConfigureBetterWeb();
		Assert.NotNull(options.TypeInfoResolver);
		Assert.DoesNotContain("warnings", JsonSerializer.Serialize(new Envelope { Content = "c" }, options));
	}

	[Fact]
	public void ConfigureBetterWeb_ChainsOntoAnExistingResolver()
	{
		var options = new JsonSerializerOptions { TypeInfoResolver = new DefaultJsonTypeInfoResolver() }.ConfigureBetterWeb();
		Assert.DoesNotContain("warnings", JsonSerializer.Serialize(new Envelope { Content = "c" }, options));
	}
}