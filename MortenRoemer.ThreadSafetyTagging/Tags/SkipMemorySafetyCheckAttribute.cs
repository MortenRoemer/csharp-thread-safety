namespace MortenRoemer.ThreadSafety;

/// <summary>
/// Indicates that the memory safety of this member should not be checked
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class SkipMemorySafetyCheckAttribute : Attribute
{
#if CORECLR
    public required string Because { get; init; }
#else
    public string Because { get; set; } = string.Empty;
#endif
}