# NSubstitute XUnitEquivalence

## Problem

`Arg.Is<T>` doesn't do deep equivalency checks and has a limited stacktrace.

```cs
var dependency = Substitute.For<IDependency>();
var sut = new Sut(dependency);

sut.Call();

dependency.Received(1).Call(Arg.Is<Argument>(p =>
    p.Id == 1 &&
    p.SubArgs.Length == 2 &&
    p.SubArgs[0].Id == 2 &&
    p.SubArgs[1].Id == 3
));

/// Message: 
/// NSubstitute.Exceptions.ReceivedCallsException : Expected to receive exactly 1 call matching:
/// 	Call(p => ((((p.Id == 1) AndAlso (ArrayLength(p.SubArgs) == 2)) AndAlso (p.SubArgs[0].Id == 2)) AndAlso (p.SubArgs[1].Id == 3)))
/// Actually received no matching calls.
/// Received 1 non-matching call (non-matching arguments indicated with '*' characters):
/// 	Call(*Argument*)
```

In this stacktrace you can't see what caused your parameters not to match. This repository provides an extension on NSubstitute, `ArgExt.IsEquivalentTo`, which uses XUnit's `Assert.Equivalent()` under the hood, and passes through a more detailed error message.

```cs
var dependency = Substitute.For<IDependency>();
var sut = new Sut(dependency);

sut.Call();

dependency.Received(1).Call(ArgExt.IsEquivalentTo(new Argument
{
    Id = 1,
    SubArgs = new SubArgument[]
    {
        new()
        {
            Id = 2
        },
        new()
        {
            Id = 3
        }
    }
}));

/// Message: 
/// NSubstitute.Exceptions.ReceivedCallsException : Expected to receive exactly 1 call matching:
/// 	Call(NSubstitute.Core.Arguments.ArgumentMatcher+GenericToNonGenericMatcherProxyWithDescribe`1[NSubstitute_XUnitEquivalence.Argument])
/// Actually received no matching calls.
/// Received 1 non-matching call (non-matching arguments indicated with '*' characters):
/// 	Call(*Argument*)
/// 		arg[0]: Assert.Equivalent() Failure: Collection value not found in member 'SubArgs'
/// 		        Expected: SubArgument { Id = 2 }
/// 		        In:       [SubArgument { Id = 3 }]

```

This code is also easier to read/write, in comparison with a lambda. Lambda's are annoying when dealing with collections or objects with deeply nested properties.

## Notes

Only use this in a project that has a dependency on XUnit.

What is `Assert.Equivalent`? From the [docs](https://xunit.net/docs/comparisons): 

> Assert.Equivalent differs from Assert.Equal in the “level of equality” that is expected from the two values. For example, Assert.Equal requires that both values are the same (or a compatible) type, whereas Assert.Equivalent will simply compare all the public fields and properties of the two values to ensure they contain the same values, even if they aren’t the same type. Equivalence comes with a “strictness” switch which allows the developer to say whether the expected value contains the complete set of values that the actual value should contain (‘strict’) vs. only a subset of values (‘not strict’). When strict comparisons are done, an “extra” properties on the actual object vs. the expected object cause failure, whereas they are ignored for non-strict comparisons.

More about Assert.Equivalent: [Assert.Equivalent GitHub Issue](https://github.com/xunit/xunit/issues/1604#issue-285614396)

## Stubs

`ArgExt.IsEquivalentTo` can also be used for stubbing. 

```cs
dependency.Call(ArgExt.IsEquivalentTo(new Argument
{
    Id = 1,
    SubArgs = new SubArgument[]
    {
        new()
        {
            Id = 2
        },
        new()
        {
            Id = 3
        }
    }
})).Returns(1);
```