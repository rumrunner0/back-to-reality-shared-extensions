using System.Text.RegularExpressions;

namespace Rumrunner0.BackToReality.SharedExtensions.Extensions;

/// <summary>Extensions for email address.</summary>
public static partial class EmailAddressExtensions
{
	/// <summary>Provides a pragmatic regular expression that matches email addresses.</summary>
	[GeneratedRegex(@"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$")]
	public static partial Regex PragmaticRegex();
}