namespace MortenRoemer.ThreadSafetyTagging.Analyzer;

public enum ThreadSafetyMode : byte
{
    Immutable = 2,
    Synchronized = 1,
    Exclusive = 0
}