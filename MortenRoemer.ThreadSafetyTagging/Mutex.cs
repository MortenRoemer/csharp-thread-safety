namespace MortenRoemer.ThreadSafety;

/// <summary>
/// Protects its content with a Mutex against concurrent access
/// </summary>
/// <typeparam name="T">The content type to protect</typeparam>
[SynchronizedMemoryAccess]
public struct Mutex<T> : IDisposable
{
    /// <summary>
    /// Creates an instance with the specified content initializer
    /// </summary>
    /// <param name="initializer">The initializer to execute on first access</param>
    public Mutex(Func<T> initializer)
    {
        _initializer = initializer;
    }
    
    private readonly Mutex _lock = new();

    private readonly Func<T> _initializer;

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
    /// Executes the specified action against the protected content
    /// </summary>
    /// <param name="action">The action to perform</param>
    /// <param name="timeOutDuration">The duration after which the action fails</param>
    /// <exception cref="TimeoutException">is thrown when it fails to access the lock</exception>
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

    /// <summary>
    /// Executes the specified action against the protected content
    /// </summary>
    /// <param name="action">The action to perform</param>
    /// <param name="timeOutDuration">The duration after which the action fails</param>
    /// <typeparam name="TReturn">The type of the return value of the action</typeparam>
    /// <returns>The value that action returns</returns>
    /// <exception cref="TimeoutException">is thrown when it fails to access the lock</exception>
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