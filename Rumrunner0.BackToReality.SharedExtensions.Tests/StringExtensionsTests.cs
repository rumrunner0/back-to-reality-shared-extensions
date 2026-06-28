using System;
using Rumrunner0.BackToReality.SharedExtensions.Extensions;
using Xunit;

namespace Rumrunner0.BackToReality.SharedExtensions.Tests;

public sealed class StringExtensionsTests
{
	[Fact]
	public void EmptinessChecks_CoverNullEmptyAndWhitespace()
	{
		Assert.True(((string?)null).IsNullOrEmpty());
		Assert.True("".IsNullOrEmpty());
		Assert.False(" ".IsNullOrEmpty());

		Assert.True("".IsEmpty());
		Assert.False(" ".IsEmpty());

		Assert.True(((string?)null).IsNullOrEmptyOrWhitespace());
		Assert.True(" \t ".IsNullOrEmptyOrWhitespace());
		Assert.False(" x ".IsNullOrEmptyOrWhitespace());

		Assert.True("".IsEmptyOrWhitespace());
		Assert.True(" \t\n".IsEmptyOrWhitespace());
		Assert.False(" x ".IsEmptyOrWhitespace());
	}

	[Theory]
	[InlineData("""{ "key": "value" }""", true)]
	[InlineData("[1, 2, 3]", true)]
	[InlineData("42", true)]
	[InlineData("{ broken", false)]
	[InlineData("", false)]
	[InlineData("   ", false)]
	[InlineData(null, false)]
	public void IsValidJson_ValidatesDocuments(string? value, bool expected)
	{
		Assert.Equal(expected, value.IsValidJson());
	}

	[Fact]
	public void SplitByWhitespace_SplitsAndDropsEmptyEntries()
	{
		Assert.Equal(new[] { "a", "b", "c" }, "a  b\tc".SplitByWhitespace());
		Assert.Empty("   ".SplitByWhitespace());
	}

	[Fact]
	public void Format_FormatsCompositeStrings()
	{
		Assert.Equal("1-two", "{0}-{1}".Format(1, "two"));
	}

	[Fact]
	public void ToGuid_ParsesAndThrows()
	{
		var guid = Guid.NewGuid();
		Assert.Equal(guid, guid.ToString().ToGuid());
		Assert.Throws<FormatException>(() => "not-a-guid".ToGuid());
	}

	[Fact]
	public void ToGuidOrNull_ReturnsNullForInvalidInput()
	{
		var guid = Guid.NewGuid();
		Assert.Equal(guid, guid.ToString().ToGuidOrNull());
		Assert.Null("not-a-guid".ToGuidOrNull());
		Assert.Null(((string?)null).ToGuidOrNull());
	}

	[Fact]
	public void TryGetBytesFromBase64String_DecodesValidInput()
	{
		Assert.True(StringExtensions.TryGetBytesFromBase64String("aGVsbG8=", out var bytes));
		Assert.Equal("hello"u8.ToArray(), bytes);

		Assert.True(StringExtensions.TryGetBytesFromBase64String("  aGVsbG8=  ", out bytes));
		Assert.Equal("hello"u8.ToArray(), bytes);

		Assert.True(StringExtensions.TryGetBytesFromBase64String("", out bytes));
		Assert.Empty(bytes);
	}

	[Fact]
	public void TryGetBytesFromBase64String_RejectsInvalidInput()
	{
		Assert.False(StringExtensions.TryGetBytesFromBase64String("###", out var bytes));
		Assert.Empty(bytes);

		Assert.False(StringExtensions.TryGetBytesFromBase64String(null, out bytes));
		Assert.Empty(bytes);

		Assert.False(StringExtensions.TryGetBytesFromBase64String("a", out bytes));
		Assert.Empty(bytes);
	}
}
