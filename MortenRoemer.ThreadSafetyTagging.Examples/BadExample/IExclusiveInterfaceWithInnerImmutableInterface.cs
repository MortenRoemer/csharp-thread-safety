using MortenRoemer.ThreadSafety;
using MortenRoemer.ThreadSafetyTagging.Examples.InterfaceExample;

namespace MortenRoemer.ThreadSafetyTagging.Examples.BadExample;

[ExclusiveMemoryAccess]
public interface IExclusiveInterfaceWithInnerImmutableInterface : IImmutableInterface
{
    
}