namespace MortenRoemer.ThreadSafety;

[SynchronizedMemoryAccess]
public struct AsyncMutex<T> : IDisposable
    where T : class
{
    public AsyncMutex(Func<Task<T>> initializer)
    {
        _initializer = initializer;
    }
    
    private readonly SemaphoreSlim _lock = new(1, 1);

    private readonly Func<Task<T>> _initializer;

    private volatile bool _initialized;
    
    [SkipMemorySafetyCheck]
    private T _instance = null!;

    public void Dispose()
    {
        _lock.Dispose();
        
        if (_initialized && _instance is IDisposable disposable)
            disposable.Dispose();
    }

    public async ValueTask Do(Func<T, ValueTask> action, TimeSpan? timeOutDuration = null)
    {
        timeOutDuration ??= Defaults.AsyncLockTimeOutDuration;
        var achievedLock = await _lock.WaitAsync(timeOutDuration.Value);
        
        if (!achievedLock)
            throw new TimeoutException($"Unable to achieve Mutex lock within the defined TimeSpan {timeOutDuration.Value:g}");
        
        try
        {
            if (!_initialized)
            {
                _instance = await _initializer();
                _initialized = true;
            }
            
            await action(_instance);
        }
        finally
        {
            _lock.Release();
        }
    }
    
    public async ValueTask<TReturn> Do<TReturn>(Func<T, ValueTask<TReturn>> action, TimeSpan? timeOutDuration = null)
    {
        timeOutDuration ??= Defaults.AsyncLockTimeOutDuration;
        var achievedLock = await _lock.WaitAsync(timeOutDuration.Value);
        
        if (!achievedLock)
            throw new TimeoutException($"Unable to achieve Mutex lock within the defined TimeSpan {timeOutDuration.Value:g}");

        try
        {
            if (!_initialized)
            {
                _instance = await _initializer();
                _initialized = true;
            }
            
            return await action(_instance);
        }
        finally
        {
            _lock.Release();
        }
    }
}