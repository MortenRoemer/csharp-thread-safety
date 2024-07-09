using System.Runtime.InteropServices;

namespace MortenRoemer.ThreadSafetyTagging.Analyzer;

public enum ThreadSafetyMode : byte
{
    Enum = 3,
    Immutable = 2,
    Synchronized = 1,
    Exclusive = 0
}

public static class ThreadSafetyModeExtension
{
    public static bool IsLessSafeThan(this ThreadSafetyMode thisMode, ThreadSafetyMode otherMode)
    {
        return thisMode switch
        {
            ThreadSafetyMode.Exclusive => otherMode != ThreadSafetyMode.Exclusive,
            ThreadSafetyMode.Synchronized => otherMode is ThreadSafetyMode.Enum or ThreadSafetyMode.Immutable,
            _ => false
        };
    }
}