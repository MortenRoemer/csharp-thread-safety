# C# Thread Safety Analyzer

## Setup
To use the thread safety tags and its analyzer install these Nuget packages:

```
dotnet add package MortenRoemer.ThreadSafetyTagging 
dotnet add package MortenRoemer.ThreadSafetyTagging.Analyzer
```

> Note: In PowerPlatform plugin development only the Analyzer package
> is necessary

## Abstract
> Note: Using this library effectively requires some basic familiarity with
> multi-threading concepts and issues.
> You can get a general overview of the topic here:
> [Thread-Safety (Wikipedia)](https://en.wikipedia.org/wiki/Thread_safety)

Detecting multi-threading-issues for any given code is notoriously hard and
this library does not try to achieve this. In C# it is and always will be easy
to write some lambda-magic and cobble some tasks and threads together, and you
will likely introduce multi-threading issues if you are not careful.

> A well-adjusted person is one who makes the same mistake twice without 
> getting nervous - Alexander Hamilton

This analyzer will not change that fact, but in my personal experience
multi-threading issues usually are not introduced by reckless multi-threading
approaches or unlucky delegate variable captures.

Instead, most of multi-threading issues I had to deal with creep up later in
the development cycle. Typically, in projects where multiple developers are
involved and iterate upon earlier designs. This makes intuitive sense: Any
experienced developer that builds a system from scratch should be able to
build without any multi-threading issues. This developer will have "perfect"
knowledge about how threads will interactive with any piece of code in this
project during this initial phase. This initial developer will make
assumptions on how all the types should be used so that the
multi-threading-safety guarantee is still uphold. As the project gets more and
more complex, more developers get onboarded and memories fade these assumptions
are lost.

This could be fixed by having comments like these in the source code:

``` csharp
// This class is intended to be used by one thread exclusively
// PLEASE DO NOT USE CONCURRENTLY
public class SomeService
{
    // ...
}
```

But this generally does not happen because:
- The information in the comment feels like common knowledge to the author
- The comment is only effective if the next developer reads all the comments 
  of all the classes they use
- These comments do not guarantee any consistency over type boundaries, so can
  not be trusted

This gets even harder if we acknowledge that C# is an object-oriented language,
so we tend to cluster smaller objects into bigger and bigger structures.

Other modern languages with focus on memory and threading safety are aware of
this and generally do not allow mixing different structures with weaker safety
guarantees. Examples are [Rust (Official Website)](https://www.rust-lang.org)
and [Go (Official Website)](https://go.dev).

The goal of this project therefore is to enable the developer to tag types with
the intended safety guarantees and the analyzer can then make sure that any
future alteration will not break those guarantees.

## The different thread safety modes

### Immutable Types
The safest types to use in any threading context are immutable types. That
means types that have only read-only properties and fields and none of those
have interior mutability.

You can annotate those with the `ImmutableMemoryAccess` attribute

Here is an example for an immutable type declaration:

``` csharp
[ImmutableMemoryAccess]
public sealed class SomeClass
{
    public SomeClass(Guid id, IEnumerable<string> labels)
    {
        Id = id;
        Labels = labels.ToImmutableArray();
    }

    public Guid Id { get; }
    
    public readonly ImmutableArray<string> Labels;
}
```

By definition immutable classes can only contain or inherit from other
immutable classes

### Synchronized Types
Chances are that your system is build out of subsystems.
Those are typically injected as dependencies into bigger systems to handle
tasks based on business logic.

As those are typically shared between different threads it is important to
make sure that these handle concurrent access. These are called synchronized
types.

> Note: It makes sense to mark interfaces **and** classes with the
> `SynchronizedMemoryAccess` attribute. Then this analyzer makes sure that
> all interfaces do not break this thread-safety guarantee.

Here is an example of a synchronized interface and class definition:

``` csharp
[SynchronizedMemoryAccess]
public interface ISomeService
{
    string AddStar();
}

[SynchronizedMemoryAccess]
public sealed class SomeService : ISomeService
{
    public SomeClass()
    {
        // You can give an initializer delegate that is executed only once
        Value = new Mutex(() => new StringBuilder());
    }

    // The Mutex helper class encapsulates other classes and prevents
    // concurrent access
    private readonly Mutex<StringBuilder> Value;
    
    public string AddStar()
    {
        return Value.Do(builder => {
            builder.Append('*');
            return builder.ToString();
        });
    }
}
```

### Exclusive Types
All other classes are only safe to use by only one thread at a time.
This is not a bad thing as excessive synchronization brings its own problems
like deadlocks and slow performance.

So most of your types will still be **exclusive** types, but you need to make
sure that they never leave their scope without proper synchronization.

Here is an example of an exclusive class definition:

``` csharp
[ExclusiveMemoryAccess]
public sealed class SomeRecord
{
   public Guid Id { get; set; }
   
   public string? Name { get; set; }
   
   public SomeRecord[]? Children { get; set; }
}
```

## Skipping Memory Checks
There are some cases where this analyzer can not determine the safety
guarantees.

Consider this field in an immutable context:

``` csharp
private readonly Func<T> Action;
```

This delegate can be either Immutable, Synchronized or Exclusive depending
on the variables that were captured during the delegate construction.

In this case you are responsible to check the safety of the type yourself.
You can then deactivate the analyzer warning for this field with the
`SkipMemorySafetyCheck` attribute:

``` csharp
[SkipMemorySafetyCheck(Because = "containing action is immutable")]
private readonly Func<T> Action;
```
