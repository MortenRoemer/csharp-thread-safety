namespace MortenRoemer.ThreadSafetyTagging.Analyzer;

public enum ThreadSafetyMode : byte
{
    Enum = 3,
    Immutable = 2,
    Synchronized = 1,
    Exclusive = 0
}