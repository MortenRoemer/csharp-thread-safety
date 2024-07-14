using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using MortenRoemer.ThreadSafetyTagging.Analyzer.ThreadSafety;

namespace MortenRoemer.ThreadSafetyTagging.Analyzer.RuleAnalyzer;

public static class MRTS0005RuleAnalyzer
{
    public static DiagnosticDescriptor Rule { get; } = new(
        id: "MRTS0005",
        title: "Current memory access mode breaks memory safety guarantees of interface",
        messageFormat: "This type memory access mode breaks the guarantees of interface {0}",
        category: "ThreadSafety",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );

    public static void Analyze(SymbolAnalysisContext context)
    {
        if (context.Symbol is not INamedTypeSymbol { TypeKind: TypeKind.Class or TypeKind.Struct or TypeKind.Interface } type)
            return;

        if (!type.GetThreadSafetyMode(out var mode))
            return;

        foreach (var interfaceType in type.Interfaces)
        {
            if (!interfaceType.GetThreadSafetyMode(out var interfaceMode))
                continue;
            
            if (mode.IsLessSafeThan(interfaceMode))
                context.ReportDiagnostic(Diagnostic.Create(Rule, type.Locations[0], interfaceType.Name));
        }
    }
}