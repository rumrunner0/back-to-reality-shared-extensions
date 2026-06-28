using System;
using System.Collections.Generic;
using Rumrunner0.BackToReality.SharedExtensions.Exceptions;
using Xunit;

namespace Rumrunner0.BackToReality.SharedExtensions.Tests;

public sealed class ArgumentExceptionExtensionsTests
{
	[Fact]
	public void Throw_AlwaysThrowsWithMessageAndParamName()
	{
		var exception = Assert.Throws<ArgumentException>(() => ArgumentExceptionExtensions.Throw("broken", "argument"));
		Assert.StartsWith("broken", exception.Message);
		Assert.Equal("argument", exception.ParamName);
	}

	[Fact]
	public void ThrowIfNull_CapturesTheArgumentExpression()
	{
		object? missing = null;
		var exception = Assert.Throws<ArgumentNullException>(() => ArgumentExceptionExtensions.ThrowIfNull(missing));
		Assert.Equal("missing", exception.ParamName);

		ArgumentExceptionExtensions.ThrowIfNull("present");
	}

	[Fact]
	public void ThrowIfNullValue_ChecksNullableStructs()
	{
		int? missing = null;
		var exception = Assert.Throws<ArgumentNullException>(() => ArgumentExceptionExtensions.ThrowIfNullValue(missing));
		Assert.Equal("missing", exception.ParamName);

		ArgumentExceptionExtensions.ThrowIfNullValue((int?)5);
	}

	[Fact]
	public void ThrowIfNullOrEmpty_ValidatesCollections()
	{
		List<int>? missing = null;
		Assert.Throws<ArgumentNullException>(() => ArgumentExceptionExtensions.ThrowIfNullOrEmpty(missing));

		var empty = new List<int>();
		var exception = Assert.Throws<ArgumentException>(() => ArgumentExceptionExtensions.ThrowIfNullOrEmpty(empty));
		Assert.Equal("empty", exception.ParamName);

		ArgumentExceptionExtensions.ThrowIfNullOrEmpty(new List<int> { 1 });
	}

	[Fact]
	public void ThrowIfEmpty_IgnoresNullCollections()
	{
		ArgumentExceptionExtensions.ThrowIfEmpty((List<int>?)null);
	}

	[Fact]
	public void ThrowIfAnyNull_ReportsTheCallerArgumentName()
	{
		var items = new List<string?> { "a", null };
		var exception = Assert.Throws<ArgumentException>(() => ArgumentExceptionExtensions.ThrowIfAnyNull(items));
		Assert.Equal("items", exception.ParamName);
		Assert.Contains("items", exception.Message);

		ArgumentExceptionExtensions.ThrowIfAnyNull(new List<string?> { "a", "b" });
		ArgumentExceptionExtensions.ThrowIfAnyNull((List<string?>?)null);
	}

	[Fact]
	public void ThrowIfNullOrEmptyOrWhiteSpace_ValidatesStrings()
	{
		string? missing = null;
		Assert.Throws<ArgumentNullException>(() => ArgumentExceptionExtensions.ThrowIfNullOrEmptyOrWhiteSpace(missing));

		var blank = "   ";
		var exception = Assert.Throws<ArgumentException>(() => ArgumentExceptionExtensions.ThrowIfNullOrEmptyOrWhiteSpace(blank));
		Assert.Equal("blank", exception.ParamName);

		ArgumentExceptionExtensions.ThrowIfNullOrEmptyOrWhiteSpace("value");
	}

	[Fact]
	public void ThrowIfNullOrEmpty_ValidatesStringsAndAcceptsNullableInput()
	{
		string? missing = null;
		Assert.Throws<ArgumentNullException>(() => ArgumentExceptionExtensions.ThrowIfNullOrEmpty(missing));

		var empty = string.Empty;
		var exception = Assert.Throws<ArgumentException>(() => ArgumentExceptionExtensions.ThrowIfNullOrEmpty(empty));
		Assert.Equal("empty", exception.ParamName);

		ArgumentExceptionExtensions.ThrowIfNullOrEmpty("value");
		ArgumentExceptionExtensions.ThrowIfEmptyOrWhiteSpace((string?)null);
		ArgumentExceptionExtensions.ThrowIfEmpty((string?)null);
	}
}
