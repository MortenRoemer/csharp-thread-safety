using MortenRoemer.ThreadSafety;

namespace MortenRoemer.ThreadSafetyTagging.Examples.InterfaceExample;

[ImmutableMemoryAccess]
public interface IImmutableInterface
{
    int Number { get; }
}