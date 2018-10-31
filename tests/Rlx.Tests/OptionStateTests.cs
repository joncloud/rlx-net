using System;
using System.Threading.Tasks;
using Xunit;
using static Rlx.Functions;

namespace Rlx.Tests
{
    public class OptionStateTests
    {
        [Fact]
        public async Task MapTests()
        {
            var expected = Some(6);
            var actual = await Some(2)
                .Map(1, (value, state) => value + state)
                .Map(3, (value, state) => Task.FromResult(value + state))
                .ToSync();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MapOrElseTests()
        {
            var expected = 2;
            var actual = None<int>()
                .MapOrElse(1, state => state + 1, (value, state) => throw new Exception());
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task AndThenTests()
        {
            var expected = Some(16);
            var actual = await Some(1)
                .AndThen(5, (value, state) => Some(value + state))
                .AndThen(10, (value, state) => Some(Task.FromResult(value + state)))
                .ToSync();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OkOrElseTests()
        {
            var expected = Error<int, string>("5");
            var actual = None<int>()
                .OkOrElse(5, state => state.ToString());
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ErrorOrElseTests()
        {
            var expected = Ok<int, string>(5);
            var actual = None<string>()
                .ErrorOrElse(5, state => state);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OrElseTests()
        {
            var expected = Some(10);
            var actual = None<int>()
                .OrElse(5, state => Some(state * 2));
            Assert.Equal(expected, actual);
        }
    }
}
