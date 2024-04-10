using NSubstitute.Core;
using NSubstitute.Core.Arguments;
using System;
using Xunit;

namespace NSubstitute_XUnitEquivalence.Lib
{
    public class XUnitEquivalentArgumentMatcher<T> : IArgumentMatcher<T>, IDescribeNonMatches
    {
        private readonly T _expected;
        private string _errorMessage;

        public XUnitEquivalentArgumentMatcher(T expected)
        {
            _expected = expected;
        }

        public string DescribeFor(object argument) => _errorMessage;

        public bool IsSatisfiedBy(T argument)
        {
            try
            {
                Assert.Equivalent(_expected, argument);
                return true;
            }
            catch (Exception e)
            {
                _errorMessage = e.Message;
                return false;
            }
        }
    }
}
