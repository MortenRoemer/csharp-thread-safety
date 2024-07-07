using MortenRoemer.ThreadSafety;

namespace MortenRoemer.ThreadSafetyTagging.Examples;

[SynchronizedMemoryAccess]
public sealed class SynchronizedDataClass
{
    // Any class that is already thead-safe can be used in a synchronized context
    public readonly HttpClient _client = new();
    
    // You can use Mutex<T>, AsyncMutex<T> and ReadWriteLock<T> instances to encapsulate exclusive data in synchronized classes
    public readonly Mutex<ExclusiveDataClass> SynchronizedData = new(() => new ExclusiveDataClass());

    // You can use primitives in synchronized classes if they are either marked as readonly or as volatile
    private volatile byte _flag;
}