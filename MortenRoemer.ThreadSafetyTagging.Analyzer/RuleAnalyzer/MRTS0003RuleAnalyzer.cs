using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using MortenRoemer.ThreadSafetyTagging.Analyzer.Platform;
using MortenRoemer.ThreadSafetyTagging.Analyzer.ThreadSafety;

namespace MortenRoemer.ThreadSafetyTagging.Analyzer.RuleAnalyzer;

public static class MRTS0003RuleAnalyzer
{
    public static DiagnosticDescriptor Rule { get; } = new(
        id: "MRTS0003",
        title: "Synchronized type with exclusive property",
        messageFormat: "Properties of synchronized types should be multi-threading safe",
        category: "ThreadSafety",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );

    public static void Analyze(SymbolAnalysisContext context)
    {
        if (context.Symbol is not IPropertySymbol property || property.HasSkipCheckAttribute())
            return;
        
        if (property.DeclaringSyntaxReferences.Length > 0)
        {
            var propertySyntax = property.DeclaringSyntaxReferences[0].GetSyntax() as PropertyDeclarationSyntax;

            if (propertySyntax?.ExpressionBody != null)
                return;
        }
        
        if (property.ContainingType is null || !property.ContainingType.GetThreadSafetyMode(out var mode) || mode != ThreadSafetyMode.Synchronized)
            return;
        
        if (property.Type.IsActivityArgument())
            return;
        
        if (!(property.IsReadOnly || (property.IsRequired && property.SetMethod!.IsInitOnly)))
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, property.Locations[0]));
            return;
        }
        
        if (!property.Type.GetThreadSafetyMode(out var innerMode) || innerMode is ThreadSafetyMode.Exclusive)
            context.ReportDiagnostic(Diagnostic.Create(Rule, property.Locations[0]));
    }
}