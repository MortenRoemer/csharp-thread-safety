using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace MortenRoemer.ThreadSafetyTagging.Analyzer.ThreadSafety;

public static class SymbolExtensions
{
    private const string ExclusiveAttributeTypeName = "ExclusiveMemoryAccessAttribute";
    private const string ImmutableAttributeTypeName = "ImmutableMemoryAccessAttribute";
    private const string SynchronizedAttributeTypeName = "SynchronizedMemoryAccessAttribute";
    private const string SkipMemorySafetyCheckAttributeTypeName = "SkipMemorySafetyCheckAttribute";

    public static string GetNamespaceQualifiedName(this ITypeSymbol typeSymbol)
    {
        var nameStack = new Stack<string>();
        nameStack.Push(typeSymbol.Name);
        var currentNamespace = typeSymbol.ContainingNamespace;

        while (!currentNamespace.IsGlobalNamespace)
        {
            nameStack.Push(currentNamespace.Name);
            currentNamespace = currentNamespace.ContainingNamespace;
        }

        var requiredLength = nameStack.Select(name => name.Length).Sum() + nameStack.Count - 1;
        var stringBuilder = new StringBuilder(requiredLength);

        while (nameStack.Count > 0)
        {
            if (stringBuilder.Length > 0)
                stringBuilder.Append('.');

            stringBuilder.Append(nameStack.Pop());
        }
        
        return stringBuilder.ToString();
    }
    
    public static bool GetThreadSafetyMode(this ITypeSymbol typeSymbol, out ThreadSafetyMode mode)
    {
        if (KnownTypes.TryGetThreadSafety(typeSymbol.GetNamespaceQualifiedName(), out var wellKnownThreadSafetyMode))
        {
            mode = wellKnownThreadSafetyMode;
            return true;
        }
        
        if (typeSymbol.BaseType is not null && typeSymbol.BaseType.SpecialType == SpecialType.System_Enum)
        {
            mode = ThreadSafetyMode.Enum;
            return true;
        }

        if (typeSymbol is INamedTypeSymbol namedType && ContainerTypes.TryGetThreadSafety(namedType.ConstructedFrom.GetNamespaceQualifiedName(), out mode))
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