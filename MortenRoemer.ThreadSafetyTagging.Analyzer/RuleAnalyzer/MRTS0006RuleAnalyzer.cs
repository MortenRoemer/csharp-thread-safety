using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using MortenRoemer.ThreadSafetyTagging.Analyzer.Extension;

namespace MortenRoemer.ThreadSafetyTagging.Analyzer.RuleAnalyzer;

public static class MRTS0006RuleAnalyzer
{
    public static DiagnosticDescriptor Rule { get; } = new(
        id: "MRTS0006",
        title: "Current memory access mode breaks memory safety guarantees of base type",
        messageFormat: "This type memory access mode breaks the guarantees of base type {0}",
        category: "ThreadSafety",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );

    public static void Analyze(SymbolAnalysisContext context)
    {
        if (context.Symbol is not INamedTypeSymbol { TypeKind: TypeKind.Class or TypeKind.Struct } type)
            return;

        if (!type.GetThreadSafetyMode(out var mode))
            return;

        if (type.BaseType is null || type.BaseType.SpecialType == SpecialType.System_Object)
            return;

        if (!type.BaseType.GetThreadSafetyMode(out var baseTypeMode))
            return;
        
        if (mode.IsLessSafeThan(baseTypeMode))
            context.ReportDiagnostic(Diagnostic.Create(Rule, type.Locations[0], type.BaseType.Name));
    }
}