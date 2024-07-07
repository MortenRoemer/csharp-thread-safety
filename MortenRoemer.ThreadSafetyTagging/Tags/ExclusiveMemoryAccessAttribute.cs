namespace MortenRoemer.ThreadSafety;

/// <summary>
/// Indicates that this type is only safe to operate in an environment where only one Thread or Task is accessing it
/// exclusively
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
public sealed class ExclusiveMemoryAccessAttribute : Attribute
{
}