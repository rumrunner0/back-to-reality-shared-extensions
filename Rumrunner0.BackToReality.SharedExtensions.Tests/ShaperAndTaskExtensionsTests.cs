using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rumrunner0.BackToReality.SharedExtensions.Extensions;
using Rumrunner0.BackToReality.SharedExtensions.Tasks;
using Xunit;

namespace Rumrunner0.BackToReality.SharedExtensions.Tests;

public sealed class ShaperAndTaskExtensionsTests
{
	[Fact]
	public void Shape_TransformsTheSource()
	{
		Assert.Equal(3, "abc".Shape(s => s.Length));
	}

	[Fact]
	public void Chain_ExecutesTheNodeAndReturnsTheSource()
	{
		var log = new List<string>();
		var source = "value";
		Assert.Same(source, source.Chain(log.Add));
		Assert.Equal(new[] { "value" }, log);
	}

	[Fact]
	public async Task ChainAsync_ExecutesTheNodeAndReturnsTheSource()
	{
		var log = new List<string>();
		var source = "value";

		var result = await source.Chain(async s =>
		{
			await Task.Yield();
			log.Add(s);
		});

		Assert.Same(source, result);
		Assert.Equal(new[] { "value" }, log);
	}

	[Fact]
	public void ChainAsync_ValidatesTheNodeEagerly()
	{
		// The exception must surface at the call, not when the returned task is awaited.
		var thrown = false;
		try
		{
			_ = "value".Chain((Func<string, Task>)null!);
		}
		catch (ArgumentNullException)
		{
			thrown = true;
		}

		Assert.True(thrown);
	}

	[Fact]
	public void Follow_ReturnsTheTarget()
	{
		Assert.Equal(42, "ignored".Follow(42));
	}

	[Fact]
	public async Task ContinueWithoutContextCapture_PreservesTaskResults()
	{
		await Task.CompletedTask.ContinueWithoutContextCapture();

		var result = await Task.FromResult(42).ContinueWithoutContextCapture();
		Assert.Equal(42, result);

		await new ValueTask().ContinueWithoutContextCapture();

		var valueResult = await new ValueTask<int>(42).ContinueWithoutContextCapture();
		Assert.Equal(42, valueResult);
	}
}
