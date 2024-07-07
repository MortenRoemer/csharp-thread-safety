namespace MortenRoemer.ThreadSafety;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class SkipMemorySafetyCheckAttribute : Attribute
{
    
}