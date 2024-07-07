namespace MortenRoemer.ThreadSafety;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
public sealed class SynchronizedMemoryAccessAttribute : Attribute
{
}