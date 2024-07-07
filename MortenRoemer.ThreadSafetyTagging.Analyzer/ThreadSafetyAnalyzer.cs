using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using MortenRoemer.ThreadSafetyTagging.Analyzer.RuleAnalyzer;

namespace MortenRoemer.ThreadSafetyTagging.Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ThreadSafetyAnalyzer : DiagnosticAnalyzer
{
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        
        context.RegisterSymbolAction(MRTS0001RuleAnalyzer.Analyze, SymbolKind.Field);
        context.RegisterSymbolAction(MRTS0002RuleAnalyzer.Analyze, SymbolKind.Property);
        context.RegisterSymbolAction(MRTS0003RuleAnalyzer.Analyze, SymbolKind.Property);
        context.RegisterSymbolAction(MRTS0004RuleAnalyzer.Analyze, SymbolKind.Field);
    }

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = new[] {
        MRTS0001RuleAnalyzer.Rule,
        MRTS0002RuleAnalyzer.Rule,
        MRTS0003RuleAnalyzer.Rule,
        MRTS0004RuleAnalyzer.Rule,
    }.ToImmutableArray();
}