namespace MortenRoemer.ThreadSafety;

public static class Defaults
{
    public static readonly TimeSpan LockTimeOutDuration = TimeSpan.FromSeconds(5);

    public static readonly TimeSpan AsyncLockTimeOutDuration = TimeSpan.FromMinutes(2);
}