using Microsoft.CodeAnalysis;

namespace MortenRoemer.ThreadSafetyTagging.Analyzer;

public static class DefinedRules
{
    public static readonly DiagnosticDescriptor ImmutableTypesShouldNotContainSynchronizedFields = new(
        id: "TS0001",
        title: "Immutable type with synchronized field",
        messageFormat: "Fields of immutable types should not be synchronized",
        category: "Safety",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
    
    public static readonly DiagnosticDescriptor ImmutableTypesShouldNotContainExclusiveFields = new(
        id: "TS0002",
        title: "Immutable type with exclusive field",
        messageFormat: "Fields of immutable types should not be exclusive",
        category: "Safety",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
    
    public static readonly DiagnosticDescriptor ImmutableFieldsShouldBeConstOrReadonly = new(
        id: "TS0003",
        title: "Immutable type with non-readonly field",
        messageFormat: "Fields of immutable types should be readonly or const",
        category: "Safety",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
    
    public static readonly DiagnosticDescriptor SynchronizedFieldsShouldBeReadonlyOrVolatile = new(
        id: "TS0004",
        title: "Synchronized type with non-readonly, non-volatile field",
        messageFormat: "Fields of synchronized types should be readonly, const or volatile",
        category: "Safety",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
    
    public static readonly DiagnosticDescriptor SynchronizedTypesShouldNotContainExclusiveFields = new(
        id: "TS0005",
        title: "Synchronized type with exclusive field",
        messageFormat: "Fields of synchronized types should not be exclusive, protect them with a Mutex for example",
        category: "Safety",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
    
    public static readonly DiagnosticDescriptor UnableToVerifyThreadSafety = new(
        id: "TS9999",
        title: "Unable to verify thread-safety",
        messageFormat: "Please attach an Attribute specifying its thread-safety",
        category: "Safety",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
}