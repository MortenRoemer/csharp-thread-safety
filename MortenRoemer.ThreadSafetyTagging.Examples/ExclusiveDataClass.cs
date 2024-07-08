using MortenRoemer.ThreadSafety;

namespace MortenRoemer.ThreadSafetyTagging.Examples;

[ExclusiveMemoryAccess]
public sealed class ExclusiveDataClass
{
    // Any property that has a setter makes a type exclusive
    public Guid? Id { get; set; }
    
    // Even properties with private/protected setters because the enable interior mutability
    public int? Number { get; private set; }

    // Collection properties are exclusive because the can be modified through their methods
    public List<int> List { get; } = [];

    // Collection properties with mutable elements are also exclusive
    public IReadOnlyList<List<int>> ListOfLists { get; } = [];

    // Any non-readonly, non-volatile field is exclusive
    private bool SomeCondition;

    [SkipMemorySafetyCheck(Because = "this list does not effect the memory safety of this class")]
    private List<int> SomeUncheckedList { get; } = [];
}