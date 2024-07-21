using System;
using Microsoft.CodeAnalysis;
using MortenRoemer.ThreadSafetyTagging.Analyzer.ThreadSafety;

namespace MortenRoemer.ThreadSafetyTagging.Analyzer.Platform;

public static class PowerPlatform
{
    private const string PluginInterfaceName = "Microsoft.Xrm.Sdk.IPlugin";
    private const string ActivityBaseClassName = "System.Activities.CodeActivity";
    private const string ActivityArgumentBaseClassName = "System.Activities.Argument";
    
    public static bool IsPowerPlatformPluginOrActivity(this ITypeSymbol typeSymbol)
    {
        return typeSymbol.IsPowerPlatformPlugin() || typeSymbol.IsPowerPlatformActivity();
    }
    
    public static bool IsPowerPlatformPlugin(this ITypeSymbol typeSymbol)
    {
        foreach (var interfaceSymbol in typeSymbol.AllInterfaces)
        {
            var interfaceName = interfaceSymbol.GetNamespaceQualifiedName();

            if (interfaceName.Equals(PluginInterfaceName, StringComparison.Ordinal))
                return true;
        }

        return false;
    }
    
    public static bool IsPowerPlatformActivity(this ITypeSymbol typeSymbol)
    {
        var parentType = typeSymbol.BaseType;

        while (parentType is not null && parentType.SpecialType != SpecialType.System_Object)
        {
            var baseTypeName = parentType.GetNamespaceQualifiedName();

            if (baseTypeName.Equals(ActivityBaseClassName, StringComparison.Ordinal))
                return true;

            parentType = parentType.BaseType;
        }

        return false;
    }

    public static bool IsActivityArgument(this ITypeSymbol typeSymbol)
    {
        var parentType = typeSymbol.BaseType;

        while (parentType is not null && parentType.SpecialType != SpecialType.System_Object)
        {
            var baseTypeName = parentType.GetNamespaceQualifiedName();

            if (baseTypeName.Equals(ActivityArgumentBaseClassName, StringComparison.Ordinal))
                return true;

            parentType = parentType.BaseType;
        }

        return false;
    }
}