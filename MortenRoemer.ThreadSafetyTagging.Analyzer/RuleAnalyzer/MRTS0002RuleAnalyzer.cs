using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using MortenRoemer.ThreadSafetyTagging.Analyzer.ThreadSafety;

namespace MortenRoemer.ThreadSafetyTagging.Analyzer.RuleAnalyzer;

public static class MRTS0002RuleAnalyzer
{
    public static DiagnosticDescriptor Rule { get; } = new(
        id: "MRTS0002",
        title: "Immutable type with non-immutable property",
        messageFormat: "Properties of immutable types should also be immutable",
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
        
        if (property.ContainingType is null || !property.ContainingType.GetThreadSafetyMode(out var mode) || mode != ThreadSafetyMode.Immutable)
            return;

        if (!(property.IsReadOnly || (property.IsRequired && property.SetMethod!.IsInitOnly)))
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, property.Locations[0]));
            return;
        }

        if (!property.Type.GetThreadSafetyMode(out var innerMode))
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, property.Locations[0]));
            return;
        }
        
        if (innerMode is not (ThreadSafetyMode.Enum or ThreadSafetyMode.Immutable))
            context.ReportDiagnostic(Diagnostic.Create(Rule, property.Locations[0]));
    }
}