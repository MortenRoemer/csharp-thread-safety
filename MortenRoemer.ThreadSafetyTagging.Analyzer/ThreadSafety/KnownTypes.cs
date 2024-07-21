using System.Collections.Generic;
using System.Collections.Immutable;

namespace MortenRoemer.ThreadSafetyTagging.Analyzer.ThreadSafety;

public static class KnownTypes
{
    private static readonly ImmutableDictionary<string, ThreadSafetyMode> Dictionary
        = new KeyValuePair<string, ThreadSafetyMode>[] {
        
        // System
        new("System.Byte", ThreadSafetyMode.Immutable),
        new("System.SByte", ThreadSafetyMode.Immutable),
        new("System.Int16", ThreadSafetyMode.Immutable),
        new("System.UInt16", ThreadSafetyMode.Immutable),
        new("System.Int32", ThreadSafetyMode.Immutable),
        new("System.UInt32", ThreadSafetyMode.Immutable),
        new("System.Int64", ThreadSafetyMode.Immutable),
        new("System.UInt64", ThreadSafetyMode.Immutable),
        new("System.Int128", ThreadSafetyMode.Immutable),
        new("System.UInt128", ThreadSafetyMode.Immutable),
        new("System.IntPtr", ThreadSafetyMode.Immutable),
        new("System.UIntPtr", ThreadSafetyMode.Immutable),
        new("System.Boolean", ThreadSafetyMode.Immutable),
        new("System.Single", ThreadSafetyMode.Immutable),
        new("System.Double", ThreadSafetyMode.Immutable),
        new("System.Decimal", ThreadSafetyMode.Immutable),
        new("System.Char", ThreadSafetyMode.Immutable),
        new("System.String", ThreadSafetyMode.Immutable),
        new("System.DateTime", ThreadSafetyMode.Immutable),
        new("System.DateOnly", ThreadSafetyMode.Immutable),
        new("System.TimeOnly", ThreadSafetyMode.Immutable),
        new("System.DateTimeOffset", ThreadSafetyMode.Immutable),
        new("System.TimeSpan", ThreadSafetyMode.Immutable),
        new("System.TimeZoneInfo", ThreadSafetyMode.Immutable),
        new("System.DBNull", ThreadSafetyMode.Immutable),
        new("System.Guid", ThreadSafetyMode.Immutable),
        new("System.Index", ThreadSafetyMode.Immutable),
        new("System.Range", ThreadSafetyMode.Immutable),
        new("System.Type", ThreadSafetyMode.Immutable),
        new("System.Uri", ThreadSafetyMode.Immutable),
        new("System.Version", ThreadSafetyMode.Immutable),
        
        // System.Net
        new("System.Net.IPAddress", ThreadSafetyMode.Immutable),
        new("System.Net.WebClient", ThreadSafetyMode.Synchronized),
        
        // System.Text
        new("System.Text.CompositeFormat", ThreadSafetyMode.Immutable),
        new("System.Text.Encoding", ThreadSafetyMode.Immutable),
        new("System.Text.EncodingInfo", ThreadSafetyMode.Immutable),
        new("System.Text.Rune", ThreadSafetyMode.Immutable),
        
        // System.Text.Json
        new("System.Text.Json.JsonEncodedText", ThreadSafetyMode.Immutable),
        new("System.Text.Json.JsonNamingPolicy", ThreadSafetyMode.Immutable),
        new("System.Text.Json.JsonProperty", ThreadSafetyMode.Immutable),
        
        // System.Threading
        new("System.Threading.AsyncLocal", ThreadSafetyMode.Synchronized),
        new("System.Threading.CancellationToken", ThreadSafetyMode.Synchronized),
        new("System.Threading.CancellationTokenSource", ThreadSafetyMode.Synchronized),
        new("System.Threading.ITimer", ThreadSafetyMode.Synchronized),
        new("System.Threading.Mutex", ThreadSafetyMode.Synchronized),
        new("System.Threading.ReaderWriterLock", ThreadSafetyMode.Synchronized),
        new("System.Threading.ReaderWriterLockSlim", ThreadSafetyMode.Synchronized),
        new("System.Threading.Semaphore", ThreadSafetyMode.Synchronized),
        new("System.Threading.SemaphoreSlim", ThreadSafetyMode.Synchronized),
        new("System.Threading.SpinLock", ThreadSafetyMode.Synchronized),
        new("System.Threading.SpinWait", ThreadSafetyMode.Synchronized),
        new("System.Threading.Thread", ThreadSafetyMode.Synchronized),
        new("System.Threading.ThreadLocal", ThreadSafetyMode.Synchronized),
        new("System.Threading.Timer", ThreadSafetyMode.Synchronized),
        new("System.Threading.WaitHandle", ThreadSafetyMode.Synchronized),
        
        // System.Threading.Tasks
        new("System.Threading.Tasks.Task", ThreadSafetyMode.Synchronized),
        new("System.Threading.Tasks.TaskScheduler", ThreadSafetyMode.Synchronized),
        new("System.Threading.Tasks.ValueTask", ThreadSafetyMode.Synchronized),
        
        // System.Threading.Channels
        new("System.Threading.Channels.Channel", ThreadSafetyMode.Synchronized),
        new("System.Threading.Channels.ChannelReader", ThreadSafetyMode.Synchronized),
        new("System.Threading.Channels.ChannelWriter", ThreadSafetyMode.Synchronized),
        
        // System.Net.Http
        new("System.Net.Http.HttpClient", ThreadSafetyMode.Synchronized),
        
        // Microsoft.Xrm.Sdk
        new("Microsoft.Xrm.Sdk.AliasedValue", ThreadSafetyMode.Immutable),
        
        
    }.ToImmutableDictionary();

    public static bool TryGetThreadSafety(string typeName, out ThreadSafetyMode mode)
    {
        return Dictionary.TryGetValue(typeName, out mode);
    }
}