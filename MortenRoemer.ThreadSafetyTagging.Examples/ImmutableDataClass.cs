using MortenRoemer.ThreadSafety;

namespace MortenRoemer.ThreadSafetyTagging.Examples;

[ImmutableMemoryAccess]
public sealed class ImmutableDataClass
{
    // any const is generally safe
    private const string ODataVersion = "4.0";
    
    public ImmutableDataClass(Guid id, ExampleEnum enumValue)
    {
        Id = id;
        EnumValueProperty = enumValue;
        EnumValueField = enumValue;
    }

    private readonly int Test = 0;
    
    // Any Enum property that is read-only is immutable
    public ExampleEnum EnumValueProperty { get; }
    
    // Any Enum property that is read-only is immutable
    public readonly ExampleEnum EnumValueField;
    
    // Any Property that is only set by the constructor and has no interior mutability is immutable
    public Guid Id { get; }
    
    // Any Property that is init only, required and has no interior mutability is immutable
    public required int? Number { get; init; }

    // Any Field that is readonly and has no interior mutability is immutable
    public readonly DateTime CreatedOn = DateTime.Now;
    
    // Any Collection Property that is readonly and has no interior mutability in its members is immutable
    public required IReadOnlyList<DateTime> PointsInTime { get; init; }
}