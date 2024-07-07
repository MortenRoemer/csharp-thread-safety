using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using MortenRoemer.ThreadSafetyTagging.Analyzer.Extension;

namespace MortenRoemer.ThreadSafetyTagging.Analyzer.RuleAnalyzer;

public static class MRTS0004RuleAnalyzer
{
    public static DiagnosticDescriptor Rule { get; } = new(
        id: "MRTS0004",
        title: "Synchronized type with exclusive field",
        messageFormat: "Fields of synchronized types should be multi-threading safe",
        category: "ThreadSafety",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );

    public static void Analyze(SymbolAnalysisContext context)
    {
        if (context.Symbol is not IFieldSymbol field || field.HasSkipCheckAttribute())
            return;
        
        if (field.ContainingType is null || !field.ContainingType.GetThreadSafetyMode(out var mode) || mode != ThreadSafetyMode.Synchronized)
            return;
        
        if (!(field.IsReadOnly || field.IsVolatile))
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, field.Locations[0]));
            return;
        }

        if (!field.Type.GetThreadSafetyMode(out var innerMode) || innerMode is ThreadSafetyMode.Exclusive)
            context.ReportDiagnostic(Diagnostic.Create(Rule, field.Locations[0]));
    }
}