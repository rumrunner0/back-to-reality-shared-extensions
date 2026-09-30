using System;

namespace Rumrunner0.BackToReality.SharedExtensions.Serialization;

/// <summary>
/// Attribute that marks a non-nullable collection member that is omitted from the JSON when it is empty.
/// Honored by the options configured through <see cref="JsonSerializerOptionsExtensions.ConfigureBetterWeb" />.
/// </summary>
/// <remarks>
/// Initialize the member to an empty collection, so an absent member reads back as empty and the two states are one on the wire.
/// The attribute on a nullable, string, or non-collection member throws <see cref="InvalidOperationException" /> the first time the type is serialized.
/// </remarks>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class JsonIgnoreWhenEmptyAttribute : Attribute;