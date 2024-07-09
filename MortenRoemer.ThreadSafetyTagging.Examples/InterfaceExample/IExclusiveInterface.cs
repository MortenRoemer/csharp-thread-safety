using MortenRoemer.ThreadSafety;

namespace MortenRoemer.ThreadSafetyTagging.Examples.InterfaceExample;

[ExclusiveMemoryAccess]
public interface IExclusiveInterface
{
    int Number { get; set; }
}