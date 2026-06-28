using System;
using System.Globalization;
using Rumrunner0.BackToReality.SharedExtensions.Environment;
using Xunit;

namespace Rumrunner0.BackToReality.SharedExtensions.Tests;

public sealed class EnvironmentVariableExtensionsTests
{
	private static string CreateVariable(string? value)
	{
		var name = $"BTR_TEST_{Guid.NewGuid():N}";
		System.Environment.SetEnvironmentVariable(name, value);
		return name;
	}

	[Fact]
	public void GetRequired_ReturnsTheValue()
	{
		var name = CreateVariable("value");
		try
		{
			Assert.Equal("value", EnvironmentVariableExtensions.GetRequired(name));
		}
		finally
		{
			System.Environment.SetEnvironmentVariable(name, null);
		}
	}

	[Fact]
	public void GetRequired_ThrowsWhenMissingOrBlank()
	{
		Assert.Throws<InvalidOperationException>(() => EnvironmentVariableExtensions.GetRequired($"BTR_TEST_{Guid.NewGuid():N}"));

		var blank = CreateVariable("   ");
		try
		{
			Assert.Throws<InvalidOperationException>(() => EnvironmentVariableExtensions.GetRequired(blank));
		}
		finally
		{
			System.Environment.SetEnvironmentVariable(blank, null);
		}

		Assert.Throws<ArgumentException>(() => EnvironmentVariableExtensions.GetRequired(" "));
	}

	[Fact]
	public void GetRequiredInt_ParsesInvariantlyRegardlessOfCulture()
	{
		var name = CreateVariable("-42");
		var culture = CultureInfo.CurrentCulture;
		try
		{
			CultureInfo.CurrentCulture = new CultureInfo("tr-TR");
			Assert.Equal(-42, EnvironmentVariableExtensions.GetRequiredInt(name));
		}
		finally
		{
			CultureInfo.CurrentCulture = culture;
			System.Environment.SetEnvironmentVariable(name, null);
		}
	}

	[Fact]
	public void GetRequiredInt_ThrowsForNonNumericValues()
	{
		var name = CreateVariable("not-a-number");
		try
		{
			Assert.Throws<InvalidOperationException>(() => EnvironmentVariableExtensions.GetRequiredInt(name));
		}
		finally
		{
			System.Environment.SetEnvironmentVariable(name, null);
		}
	}
}
