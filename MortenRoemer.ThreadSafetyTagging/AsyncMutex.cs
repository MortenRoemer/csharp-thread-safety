namespace MortenRoemer.ThreadSafety;

/// <summary>
/// Protects a type against concurrent access by using a SemaphoreSlim.
/// </summary>
/// <typeparam name="T">The type to protect against concurrent access</typeparam>
[SynchronizedMemoryAccess]
public struct AsyncMutex<T> : IDisposable
{
    /// <summary>
    /// Creates a new instance with the specified initializer delegate
    /// </summary>
    /// <param name="initializer">A delegate to initialize the protected content</param>
    public AsyncMutex(Func<Task<T>> initializer)
    {
        _initializer = initializer;
    }
    
    private readonly SemaphoreSlim _lock = new(1, 1);

    private readonly Func<Task<T>> _initializer;

    private volatile bool _initialized;
    
    [SkipMemorySafetyCheck]
    private T _instance;

    public void Dispose()
    {
        _lock.Dispose();
        
        if (_initialized && _instance is IDisposable disposable)
            disposable.Dispose();
    }

    /// <summary>
    /// Executes the specified action on its content in a matter that is safe in a multi-threading context
    /// </summary>
    /// <param name="action">The action to execute</param>
    /// <param name="timeOutDuration">The time out duration at which this action fails</param>
    /// <exception cref="TimeoutException">is thrown if no lock was able to be acquired</exception>
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
    
    /// <summary>
    /// Executes the specified action on its content in a matter that is safe in a multi-threading context
    /// </summary>
    /// <param name="action">The action to execute</param>
    /// <param name="timeOutDuration">The time out duration at which this action fails</param>
    /// <typeparam name="TReturn">The type of the return value of action</typeparam>
    /// <returns>The value that the action returns</returns>
    /// <exception cref="TimeoutException">is thrown if no lock was able to be acquired</exception>
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