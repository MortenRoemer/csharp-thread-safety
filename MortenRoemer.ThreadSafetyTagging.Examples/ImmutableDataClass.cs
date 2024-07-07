using MortenRoemer.ThreadSafety;

namespace MortenRoemer.ThreadSafetyTagging.Examples;

[ImmutableMemoryAccess]
public sealed class ImmutableDataClass
{
    public ImmutableDataClass(Guid id)
    {
        Id = id;
    }

    private readonly int Test = 0;
    
    // Any Property that is only set by the constructor and has no interior mutability is immutable
    public Guid Id { get; }
    
    // Any Property that is init only, required and has no interior mutability is immutable
    public required int? Number { get; init; }

    // Any Field that is readonly and has no interior mutability is immutable
    public readonly DateTime CreatedOn = DateTime.Now;
    
    // Any Collection Property that is readonly and has no interior mutability in its members is immutable
    public required IReadOnlyList<DateTime> PointsInTime { get; init; }
}