namespace MortenRoemer.ThreadSafety;

/// <summary>
/// Indicates that this type has no interior mutability and is therefore safe in any threading context.
/// It is strongly advised to provide a reason with the <see cref="Reason"/> property.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = false)]
public sealed class ImmutableMemoryAccessAttribute : Attribute
{
    /// <summary>
    /// Provides a message to other developers on the reasons for the memory access mode of this type.
    /// This string is not used anywhere and is ignored by the analyzer.
    /// </summary>
#if CORECLR
    public required string Reason { get; init; }
#else
    public string Reason { get; set; } = string.Empty;
#endif
}