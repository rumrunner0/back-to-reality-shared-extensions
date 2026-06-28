using System;
using System.IO;
using Rumrunner0.BackToReality.SharedExtensions.Exceptions;
using Xunit;

namespace Rumrunner0.BackToReality.SharedExtensions.Tests;

public sealed class ExceptionExtensionsTests
{
	[Fact]
	public void JoinMessages_JoinsTheLinearChain()
	{
		var exception = new InvalidOperationException("outer", new IOException("middle", new TimeoutException("inner")));
		Assert.Equal("outer | middle | inner", exception.JoinMessages(" | "));
	}

	[Fact]
	public void JoinMessages_IncludesEveryAggregateBranch()
	{
		var aggregate = new AggregateException(new TimeoutException("first"), new InvalidOperationException("second", new IOException("deep")));
		var joined = aggregate.JoinMessages(" | ");
		Assert.Contains("first", joined);
		Assert.Contains("second", joined);
		Assert.Contains("deep", joined);
	}

	[Fact]
	public void JoinMessages_ValidatesTheSeparator()
	{
		var exception = new InvalidOperationException("message");
		Assert.Throws<ArgumentException>(() => exception.JoinMessages(string.Empty));
	}

	[Fact]
	public void IsOrHasInner_FindsTheSourceItself()
	{
		var exception = new TimeoutException("t");
		Assert.Same(exception, exception.IsOrHasInner<TimeoutException>());
	}

	[Fact]
	public void IsOrHasInner_FindsDeepInnerExceptions()
	{
		var inner = new TimeoutException("t");
		var exception = new InvalidOperationException("outer", new IOException("middle", inner));
		Assert.Same(inner, exception.IsOrHasInner<TimeoutException>());
		Assert.Null(exception.IsOrHasInner<ArgumentException>());
	}

	[Fact]
	public void IsOrHasInner_SearchesAllAggregateBranches()
	{
		var target = new TimeoutException("t");
		var aggregate = new AggregateException(new IOException("io"), new InvalidOperationException("wrap", target));
		Assert.Same(target, aggregate.IsOrHasInner<TimeoutException>());
	}

	[Fact]
	public void HasInner_IgnoresTheSourceButSearchesAllBranches()
	{
		var target = new TimeoutException("t");
		var source = new TimeoutException("outer", new InvalidOperationException("wrap", target));
		Assert.Same(target, source.HasInner<TimeoutException>());

		var aggregate = new AggregateException(new IOException("io"), new InvalidOperationException("wrap", target));
		Assert.Same(target, aggregate.HasInner<TimeoutException>());
		Assert.Null(new TimeoutException("alone").HasInner<TimeoutException>());
	}

	[Fact]
	public void JoinMessages_PreservesDepthFirstOrderAcrossAggregateBranches()
	{
		// The whole subtree of the first branch must appear before the second branch starts.
		var branchA = new InvalidOperationException("wrapA", new TimeoutException("deepA"));
		var branchB = new IOException("lastB");
		var aggregate = new AggregateException(branchA, branchB);

		var parts = aggregate.JoinMessages("|").Split('|');

		Assert.Equal(new[] { aggregate.Message, "wrapA", "deepA", "lastB" }, parts);
	}

	[Fact]
	public void JoinMessages_TraversesNestedAggregates()
	{
		var nested = new AggregateException(new TimeoutException("x1"), new IOException("y1"));
		var aggregate = new AggregateException(nested, new InvalidOperationException("z1"));

		var parts = aggregate.JoinMessages("|").Split('|');

		Assert.Equal(new[] { aggregate.Message, nested.Message, "x1", "y1", "z1" }, parts);
	}

	[Fact]
	public void JoinMessages_HandlesEmptyAggregates()
	{
		var aggregate = new AggregateException();
		Assert.Equal(aggregate.Message, aggregate.JoinMessages("|"));
	}

	[Fact]
	public void JoinMessages_HandlesDeepChains()
	{
		var exception = new Exception("0");
		for (var i = 1; i < 10_000; i++)
		{
			exception = new Exception(i.ToString(), exception);
		}

		var parts = exception.JoinMessages("|").Split('|');

		Assert.Equal(10_000, parts.Length);
		Assert.Equal("9999", parts[0]);
		Assert.Equal("0", parts[^1]);
	}
}
