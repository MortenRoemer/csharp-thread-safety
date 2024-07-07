namespace MortenRoemer.ThreadSafety;

/// <summary>
/// Provides defaults for this package
/// </summary>
public static class Defaults
{
    /// <summary>
    /// The default lock time out duration for synchronous actions
    /// </summary>
    public static readonly TimeSpan LockTimeOutDuration = TimeSpan.FromSeconds(5);

    /// <summary>
    /// The default lock time out duration for asynchronous actions
    /// </summary>
    public static readonly TimeSpan AsyncLockTimeOutDuration = TimeSpan.FromMinutes(2);
}