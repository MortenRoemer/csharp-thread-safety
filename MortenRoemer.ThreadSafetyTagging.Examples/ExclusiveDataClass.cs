using MortenRoemer.ThreadSafety;
using MortenRoemer.ThreadSafetyTagging.Examples.InterfaceExample;

namespace MortenRoemer.ThreadSafetyTagging.Examples;

[ExclusiveMemoryAccess]
public sealed class ExclusiveDataClass : IExclusiveInterface
{
    // Any property that has a setter makes a type exclusive
    public int Number { get; set; }

    // Collection properties are exclusive because the can be modified through their methods
    public List<int> List { get; } = [];

    // Collection properties with mutable elements are also exclusive
    public IReadOnlyList<List<int>> ListOfLists { get; } = [];

    // Any non-readonly field is exclusive
    public bool SomeCondition;

    [SkipMemorySafetyCheck(Because = "this list does not effect the memory safety of this class")]
    private List<int> SomeUncheckedList { get; } = [];
}