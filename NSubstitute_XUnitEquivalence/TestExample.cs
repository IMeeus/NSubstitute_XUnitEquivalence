using NSubstitute;
using NSubstitute_XUnitEquivalence.Lib;
using Xunit;

namespace NSubstitute_XUnitEquivalence
{
    public class TestExample
    {
        [Fact]
        public void Test()
        {
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
        }
    }

    public class Sut
    {
        private readonly IDependency _dependency;

        public Sut(IDependency dependency)
        {
            _dependency = dependency;
        }

        public void Call()
        {
            _dependency.Call(new Argument
            {
                Id = 1,
                SubArgs = new SubArgument[]
                {
                    new()
                    {
                        Id = 3
                    }
                }
            });
        }
    }

    public interface IDependency
    {
        void Call(Argument arg);
    }

    public class Argument
    {
        public int Id { get; set; }
        public SubArgument[] SubArgs { get; set; }
    }

    public class SubArgument
    {
        public int Id { get; set; }
    }
}
