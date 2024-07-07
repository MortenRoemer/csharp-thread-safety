namespace MortenRoemer.ThreadSafety;

[SynchronizedMemoryAccess]
public readonly struct ReadWriteLock<T> : IDisposable
where T : class
{
    public ReadWriteLock(T instance)
    {
        _instance = instance;
    }
    
    private readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.NoRecursion);

    [SkipMemorySafetyCheck]
    private readonly T _instance;
    
    public void Dispose()
    {
        _lock.Dispose();
        
        if (_instance is IDisposable disposable)
            disposable.Dispose();
    }

    public TReturn Read<TReturn>(Func<T, TReturn> action, TimeSpan? timeOutDuration = null)
    {
        timeOutDuration ??= Defaults.LockTimeOutDuration;
        var achievedLock = _lock.TryEnterReadLock(timeOutDuration.Value);
        
        if (!achievedLock)
            throw new TimeoutException($"Unable to achieve lock within the defined TimeSpan {timeOutDuration.Value:g}");

        try
        {
            return action(_instance);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public void Write(Action<T> action, TimeSpan? timeOutDuration = null)
    {
        timeOutDuration ??= Defaults.LockTimeOutDuration;
        var achievedLock = _lock.TryEnterWriteLock(timeOutDuration.Value);
        
        if (!achievedLock)
            throw new TimeoutException($"Unable to achieve lock within the defined TimeSpan {timeOutDuration.Value:g}");

        try
        {
            action(_instance);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }
}