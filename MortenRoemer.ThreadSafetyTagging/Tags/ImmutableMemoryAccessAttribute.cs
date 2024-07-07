namespace MortenRoemer.ThreadSafety;

/// <summary>
/// Indicates that this type has no interior mutability and is therefore safe in any threading context
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
public sealed class ImmutableMemoryAccessAttribute : Attribute
{
}