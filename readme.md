# NSubstitute XUnitEquivalence

## Motivation

Can't do deep equivalency checks in NSubstitute...

## Comparison with traditional NSubstitute

### Before

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

### After

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



