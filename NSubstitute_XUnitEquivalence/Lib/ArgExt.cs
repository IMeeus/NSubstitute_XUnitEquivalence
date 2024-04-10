using NSubstitute.Core.Arguments;

namespace NSubstitute_XUnitEquivalence.Lib
{
    public static class ArgExt
    {
        public static ref T IsEquivalentTo<T>(T expected) => ref ArgumentMatcher.Enqueue(new XUnitEquivalentArgumentMatcher<T>(expected));
    }
}
