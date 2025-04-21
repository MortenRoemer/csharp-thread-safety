using System.Collections.Generic;
using System.Collections.Immutable;

namespace MortenRoemer.ThreadSafetyTagging.Analyzer.ThreadSafety;

public static class ContainerTypes
{
    private static readonly ImmutableDictionary<string, ThreadSafetyMode> Dictionary
        = new KeyValuePair<string, ThreadSafetyMode>[] {
            new("System.Nullable", ThreadSafetyMode.Immutable),
            new("System.Collections.Generic.KeyValuePair", ThreadSafetyMode.Immutable),
            new("System.Collections.Generic.IReadOnlyCollection", ThreadSafetyMode.Immutable),
            new("System.Collections.Generic.IReadOnlyDictionary", ThreadSafetyMode.Immutable),
            new("System.Collections.Generic.IReadOnlyList", ThreadSafetyMode.Immutable),
            new("System.Collections.Generic.IReadOnlySet", ThreadSafetyMode.Immutable),
            new("System.Collections.Immutable.ImmutableArray", ThreadSafetyMode.Immutable),
            new("System.Collections.Immutable.ImmutableDictionary", ThreadSafetyMode.Immutable),
            new("System.Collections.Immutable.ImmutableSortedDictionary", ThreadSafetyMode.Immutable),
            new("System.Collections.Immutable.ImmutableList", ThreadSafetyMode.Immutable),
            new("System.Collections.Immutable.ImmutableQueue", ThreadSafetyMode.Immutable),
            new("System.Collections.Immutable.ImmutableHashSet", ThreadSafetyMode.Immutable),
            new("System.Collections.Immutable.ImmutableSortedSet", ThreadSafetyMode.Immutable),
            new("System.Collections.Immutable.ImmutableStack", ThreadSafetyMode.Immutable),
            new("System.Collections.Frozen.FrozenDictionary", ThreadSafetyMode.Immutable),
            new("System.Collections.Frozen.FrozenSet", ThreadSafetyMode.Immutable),
            new("System.ReadOnlyMemory", ThreadSafetyMode.Immutable),
            new("System.ReadOnlySpan", ThreadSafetyMode.Immutable),
            new("System.Collections.Concurrent.ConcurrentBag", ThreadSafetyMode.Synchronized),
            new("System.Collections.Concurrent.ConcurrentDictionary", ThreadSafetyMode.Synchronized),
            new("System.Collections.Concurrent.ConcurrentQueue", ThreadSafetyMode.Synchronized),
            new("System.Collections.Concurrent.ConcurrentStack", ThreadSafetyMode.Synchronized),
        }.ToImmutableDictionary();

    public static bool TryGetThreadSafety(string name, out ThreadSafetyMode mode)
    {
        return Dictionary.TryGetValue(name, out mode);
    }
}