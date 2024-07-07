using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using MortenRoemer.ThreadSafetyTagging.Analyzer.Extension;

namespace MortenRoemer.ThreadSafetyTagging.Analyzer.RuleAnalyzer;

public static class MRTS0001RuleAnalyzer
{
    public static DiagnosticDescriptor Rule { get; } = new(
        id: "MRTS0001",
        title: "Immutable type with non-immutable field",
        messageFormat: "Fields of immutable types should also be immutable",
        category: "ThreadSafety",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
    
    public static void Analyze(SymbolAnalysisContext context)
    {
        if (context.Symbol is not IFieldSymbol field || field.HasSkipCheckAttribute())
            return;
        
        if (field.ContainingType is null || !field.ContainingType.GetThreadSafetyMode(out var mode) || mode != ThreadSafetyMode.Immutable)
            return;
        
        if (!field.IsReadOnly)
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, field.Locations[0]));
            return;
        }

        if (!field.Type.GetThreadSafetyMode(out var innerMode) || innerMode != ThreadSafetyMode.Immutable)
            context.ReportDiagnostic(Diagnostic.Create(Rule, field.Locations[0]));
    }
}