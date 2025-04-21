namespace MortenRoemer.ThreadSafety;

/// <summary>
/// Indicates that the developer verifies the memory safety check and is to be skipped by the analyzer.
/// It is strongly advised to provide a reason with the <see cref="Because"/> property.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class SkipMemorySafetyCheckAttribute : Attribute
{
    /// <summary>
    /// Provides a reason to other developers why the memory safety of this member is enough for the specified use-case.
    /// This property exists only for documentation reasons and is ignored by the analyzer.
    /// </summary>
#if CORECLR
    public required string Because { get; init; }
#else
    public string Because { get; set; } = string.Empty;
#endif
}