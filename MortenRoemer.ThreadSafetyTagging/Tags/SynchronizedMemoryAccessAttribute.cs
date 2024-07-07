namespace MortenRoemer.ThreadSafety;

/// <summary>
/// Indicates that this type is safe to use in multi-threading contexts, because it uses strategies like Mutexes and
/// locks to ensure its safety
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
public sealed class SynchronizedMemoryAccessAttribute : Attribute
{
}