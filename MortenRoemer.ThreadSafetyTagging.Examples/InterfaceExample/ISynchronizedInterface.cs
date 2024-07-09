using MortenRoemer.ThreadSafety;

namespace MortenRoemer.ThreadSafetyTagging.Examples.InterfaceExample;

[SynchronizedMemoryAccess]
public interface ISynchronizedInterface
{
    int Number { get; }
}