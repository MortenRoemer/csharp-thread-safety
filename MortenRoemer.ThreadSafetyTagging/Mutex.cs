namespace MortenRoemer.ThreadSafety;

[SynchronizedMemoryAccess]
public struct Mutex<T> : IDisposable
where T : class
{
    public Mutex(Func<T> initializer)
    {
        _initializer = initializer;
    }
    
    private readonly Mutex _lock = new();

    private readonly Func<T> _initializer;

    private volatile bool _initialized;

    [SkipMemorySafetyCheck]
    private T _instance = null!;
    
    public void Dispose()
    {
        _lock.Dispose();

        if (_initialized && _instance is IDisposable disposable)
            disposable.Dispose();
    }
    
    public void Do(Action<T> action, TimeSpan? timeOutDuration = null)
    {
        timeOutDuration ??= Defaults.LockTimeOutDuration;
        var achievedLock = _lock.WaitOne(timeOutDuration.Value);

        if (!achievedLock)
            throw new TimeoutException($"Unable to achieve Mutex lock within the defined TimeSpan {timeOutDuration.Value:g}");
        
        try
        {
            if (!_initialized)
            {
                _instance = _initializer();
                _initialized = true;
            }
            
            action(_instance);
        }
        finally
        {
            _lock.ReleaseMutex();
        }
    }

    public TReturn Do<TReturn>(Func<T, TReturn> action, TimeSpan? timeOutDuration = null)
    {
        timeOutDuration ??= Defaults.LockTimeOutDuration;
        var achievedLock = _lock.WaitOne(timeOutDuration.Value);

        if (!achievedLock)
            throw new TimeoutException($"Unable to achieve Mutex lock within the defined TimeSpan {timeOutDuration.Value:g}");
        
        try
        {
            if (!_initialized)
            {
                _instance = _initializer();
                _initialized = true;
            }
            
            return action(_instance);
        }
        finally
        {
            _lock.ReleaseMutex();
        }
    }
}