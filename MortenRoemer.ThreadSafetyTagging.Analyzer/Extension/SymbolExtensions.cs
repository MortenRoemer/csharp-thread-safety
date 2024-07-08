using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace MortenRoemer.ThreadSafetyTagging.Analyzer.Extension;

public static class SymbolExtensions
{
    private const string ExclusiveAttributeTypeName = "ExclusiveMemoryAccessAttribute";
    private const string ImmutableAttributeTypeName = "ImmutableMemoryAccessAttribute";
    private const string SynchronizedAttributeTypeName = "SynchronizedMemoryAccessAttribute";
    private const string SkipMemorySafetyCheckAttributeTypeName = "SkipMemorySafetyCheckAttribute";

    private static readonly ImmutableDictionary<string, ThreadSafetyMode> WellKnownTypes
        = new KeyValuePair<string, ThreadSafetyMode>[] {
        new("Byte", ThreadSafetyMode.Immutable),
        new("SByte", ThreadSafetyMode.Immutable),
        new("Int16", ThreadSafetyMode.Immutable),
        new("UInt16", ThreadSafetyMode.Immutable),
        new("Int32", ThreadSafetyMode.Immutable),
        new("UInt32", ThreadSafetyMode.Immutable),
        new("Int64", ThreadSafetyMode.Immutable),
        new("UInt64", ThreadSafetyMode.Immutable),
        new("Int128", ThreadSafetyMode.Immutable),
        new("UInt128", ThreadSafetyMode.Immutable),
        new("IntPtr", ThreadSafetyMode.Immutable),
        new("UIntPtr", ThreadSafetyMode.Immutable),
        new("Boolean", ThreadSafetyMode.Immutable),
        new("Single", ThreadSafetyMode.Immutable),
        new("Double", ThreadSafetyMode.Immutable),
        new("Decimal", ThreadSafetyMode.Immutable),
        new("Char", ThreadSafetyMode.Immutable),
        new("String", ThreadSafetyMode.Immutable),
        new("DateTime", ThreadSafetyMode.Immutable),
        new("DateOnly", ThreadSafetyMode.Immutable),
        new("TimeOnly", ThreadSafetyMode.Immutable),
        new("DateTimeOffset", ThreadSafetyMode.Immutable),
        new("TimeSpan", ThreadSafetyMode.Immutable),
        new("TimeZoneInfo", ThreadSafetyMode.Immutable),
        new("DBNull", ThreadSafetyMode.Immutable),
        new("Guid", ThreadSafetyMode.Immutable),
        new("HashCode", ThreadSafetyMode.Immutable),
        new("Index", ThreadSafetyMode.Immutable),
        new("Range", ThreadSafetyMode.Immutable),
        new("Type", ThreadSafetyMode.Immutable),
        new("Uri", ThreadSafetyMode.Immutable),
        new("Version", ThreadSafetyMode.Immutable),
        new("Mutex", ThreadSafetyMode.Synchronized),
        new("Semaphore", ThreadSafetyMode.Synchronized),
        new("SemaphoreSlim", ThreadSafetyMode.Synchronized),
        new("HttpClient", ThreadSafetyMode.Synchronized),
    }.ToImmutableDictionary();

    private static readonly ImmutableDictionary<string, ThreadSafetyMode> ContainerTypes
        = new KeyValuePair<string, ThreadSafetyMode>[] {
            new("Nullable", ThreadSafetyMode.Immutable),
            new("KeyValuePair", ThreadSafetyMode.Immutable),
            new("IReadOnlyCollection", ThreadSafetyMode.Immutable),
            new("IReadOnlyDictionary", ThreadSafetyMode.Immutable),
            new("IReadOnlyList", ThreadSafetyMode.Immutable),
            new("IReadOnlySet", ThreadSafetyMode.Immutable),
            new("ImmutableArray", ThreadSafetyMode.Immutable),
            new("ImmutableDictionary", ThreadSafetyMode.Immutable),
            new("ImmutableSortedDictionary", ThreadSafetyMode.Immutable),
            new("ImmutableList", ThreadSafetyMode.Immutable),
            new("ImmutableQueue", ThreadSafetyMode.Immutable),
            new("ImmutableHashSet", ThreadSafetyMode.Immutable),
            new("ImmutableSortedSet", ThreadSafetyMode.Immutable),
            new("ImmutableStack", ThreadSafetyMode.Immutable),
            new("FrozenDictionary", ThreadSafetyMode.Immutable),
            new("FrozenSet", ThreadSafetyMode.Immutable),
            new("ConcurrentBag", ThreadSafetyMode.Synchronized),
            new("ConcurrentDictionary", ThreadSafetyMode.Synchronized),
            new("ConcurrentQueue", ThreadSafetyMode.Synchronized),
            new("ConcurrentStack", ThreadSafetyMode.Synchronized),
        }.ToImmutableDictionary();
    
    public static bool GetThreadSafetyMode(this ITypeSymbol typeSymbol, out ThreadSafetyMode mode)
    {
        if (WellKnownTypes.TryGetValue(typeSymbol.Name, out var wellKnownThreadSafetyMode))
        {
            mode = wellKnownThreadSafetyMode;
            return true;
        }
        
        if (typeSymbol.BaseType is not null && typeSymbol.BaseType.SpecialType == SpecialType.System_Enum)
        {
            mode = ThreadSafetyMode.Enum;
            return true;
        }

        if (typeSymbol is INamedTypeSymbol namedType && ContainerTypes.TryGetValue(namedType.ConstructedFrom.Name, out mode))
        {
            foreach (var typeArgument in namedType.TypeArguments)
            {
                var innerMode = typeArgument.GetThreadSafetyMode(out var typeArgumentMode)
                    ? typeArgumentMode
                    : ThreadSafetyMode.Exclusive;

                mode = CombineThreadSafetyModes(mode, innerMode);
            }

            return true;
        }
        
        foreach (var attribute in typeSymbol.GetAttributes())
        {
            if (attribute.AttributeClass is null)
                continue;
            
            var attributeClassName = attribute.AttributeClass.Name;

            switch (attributeClassName)
            {
                case ExclusiveAttributeTypeName:
                    mode = ThreadSafetyMode.Exclusive;
                    return true;
                    
                case SynchronizedAttributeTypeName:
                    mode = ThreadSafetyMode.Synchronized;
                    return true;
                    
                case ImmutableAttributeTypeName:
                    mode = ThreadSafetyMode.Immutable;
                    return true;
            }
        }

        mode = default;
        return false;
    }

    public static bool HasSkipCheckAttribute(this ISymbol symbol)
    {
        return symbol.GetAttributes()
            .Any(attribute => attribute.AttributeClass?.Name == SkipMemorySafetyCheckAttributeTypeName);
    }

    private static ThreadSafetyMode CombineThreadSafetyModes(ThreadSafetyMode left, ThreadSafetyMode right)
    {
        if (left is ThreadSafetyMode.Exclusive || right is ThreadSafetyMode.Exclusive)
            return ThreadSafetyMode.Exclusive;

        if (left is ThreadSafetyMode.Synchronized || right is ThreadSafetyMode.Synchronized)
            return ThreadSafetyMode.Synchronized;

        return ThreadSafetyMode.Immutable;
    }
}