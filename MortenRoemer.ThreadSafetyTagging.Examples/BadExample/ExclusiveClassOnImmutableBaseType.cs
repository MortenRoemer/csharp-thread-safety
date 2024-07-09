using MortenRoemer.ThreadSafety;

namespace MortenRoemer.ThreadSafetyTagging.Examples.BadExample;

[ExclusiveMemoryAccess]
public sealed class ExclusiveClassOnImmutableBaseType : ImmutableDataClass
{
    public ExclusiveClassOnImmutableBaseType(int number, ExampleEnum enumValue) : base(number, enumValue)
    {
    }
}