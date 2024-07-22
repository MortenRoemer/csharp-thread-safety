using System.Text.RegularExpressions;
using MortenRoemer.ThreadSafety;
using MortenRoemer.ThreadSafetyTagging.Examples.InterfaceExample;

namespace MortenRoemer.ThreadSafetyTagging.Examples;

[ImmutableMemoryAccess]
public class ImmutableDataClass : IImmutableInterface
{
    // any const is generally safe
    private const string ODataVersion = "4.0";

    private static readonly Regex Pattern = new Regex(".*"); 
    
    public ImmutableDataClass(int number, ExampleEnum enumValue)
    {
        Number = number;
        EnumValueProperty = enumValue;
        EnumValueField = enumValue;
    }
    
    // Any Enum property that is read-only is immutable
    public ExampleEnum EnumValueProperty { get; }
    
    // Any Enum property that is read-only is immutable
    public readonly ExampleEnum EnumValueField;
    
    // Any Property that is only set by the constructor and has no interior mutability is immutable
    public int Number { get; }

    // Any Field that is readonly and has no interior mutability is immutable
    public readonly DateTime CreatedOn = DateTime.Now;
    
    // Any Collection Property that is readonly and has no interior mutability in its members is immutable
    public required IReadOnlyList<DateTime> PointsInTime { get; init; }
}