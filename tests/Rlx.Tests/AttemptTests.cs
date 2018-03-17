using System;
using Xunit;
using static Rlx.Functions;

namespace Rlx.Tests
{
    public class AttemptTests
    {
        [Fact]
        public void ImplicitOperator_ShouldReturnErrorGivenExceptionThrown()
        {
            var exception = new Exception();
            Result<int, Exception> actual = new Attempt<int>(() => throw exception);
            Assert.Equal(Error<int, Exception>(exception), actual);
        }

        [Fact]
        public void ImplicitOperator_ShouldReturnValueGivenNoExceptionThrown()
        {
            var value = 1;
            Result<int, Exception> actual = new Attempt<int>(() => value);
            Assert.Equal(Ok<int, Exception>(value), actual);
        }

        [Fact]
        public void Catch_ShouldReturnValueGivenNoExceptionThrown()
        {
            var value = 1;
            var actual = new Attempt<int>(() => value)
                .Catch<InvalidOperationException>();
            Assert.Equal(Ok<int, Exception>(value), actual);
        }

        [Fact]
        public void Catch_ShouldReturnErrorGivenExceptionThrown()
        {
            var exception = new InvalidOperationException();
            var actual = new Attempt<int>(() => throw exception)
                .Catch<InvalidOperationException>();
            Assert.Equal(Error<int, Exception>(exception), actual);
        }

        [Fact]
        public void Catch_ShouldThrowGivenMismatchedExceptionTypes()
        {
            var expected = new FormatException();
            var actual = Assert.Throws<FormatException>(() =>
                new Attempt<int>(() => throw expected)
                .Catch<InvalidOperationException>()
            );
            Assert.Equal(expected, actual);
        }
    }
}
