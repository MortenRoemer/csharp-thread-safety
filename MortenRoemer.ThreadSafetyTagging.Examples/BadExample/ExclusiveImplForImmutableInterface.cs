using MortenRoemer.ThreadSafety;
using MortenRoemer.ThreadSafetyTagging.Examples.InterfaceExample;

namespace MortenRoemer.ThreadSafetyTagging.Examples.BadExample;

[ExclusiveMemoryAccess]
public class ExclusiveImplForImmutableInterface : IImmutableInterface
{
    public int Number { get; set; }
}