using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Rlx.Functions;

namespace Rlx.Tests
{
    public class ResultTests
    {
        [Fact]
        public void IsOkTests()
        {
            Assert.True(Ok<int, string>(-3).IsOk);
            Assert.False(Error<int, string>("Some error message").IsOk);
        }

        [Fact]
        public void IsErrorTests()
        {
            Assert.False(Ok<int, string>(-3).IsError);
            Assert.True(Error<int, string>("Some error message").IsError);
        }

        [Fact]
        public void OkTests()
        {
            Assert.Equal(Some(2), Ok<int, string>(2).Ok());
            Assert.Equal(None<int>(), Error<int, string>("Nothing here").Ok());
        }

        [Fact]
        public void ErrorTests()
        {
            Assert.Equal(None<string>(), Ok<int, string>(2).Error());
            Assert.Equal(Some("Nothing here"), Error<int, string>("Nothing here").Error());
        }

        [Fact]
        public void MapTests()
        {
            const string line = "1\n2\n3\n4\n";
            var values = new[] {
                Ok<int, string>(2),
                Ok<int, string>(4),
                Ok<int, string>(6),
                Ok<int, string>(8)
            };
            var nums = line.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(values, nums.Select(num => Parse.Int32(num).OkOr("Invalid number").Map(i => i * 2)));
        }

        [Fact]
        public async Task MapAsyncTests()
        {
            const string line = "1\n2\n3\n4\n";
            var values = new[] {
                Ok<int, string>(2),
                Ok<int, string>(4),
                Ok<int, string>(6),
                Ok<int, string>(8)
            };
            var nums = line.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var tasks = nums.Select(num => Parse.Int32(num).OkOr("Invalid Number").Map(i => Task.FromResult(i * 2)).ToSync());
            Assert.Equal(values, await Task.WhenAll(tasks));
        }

        [Fact]
        public void MapErrorTests()
        {
            string Stringify(int x) => $"error code: {x}";

            Assert.Equal(Ok<int, string>(2), Ok<int, int>(2).MapError(Stringify));
            Assert.Equal(Error<int, string>("error code: 13"), Error<int, int>(13).MapError(Stringify));
        }

        [Fact]
        public async Task MapErrorAsyncTests()
        {
            Task<string> Stringify(int x) => Task.FromResult($"error code: {x}");

            Assert.Equal(Ok<int, string>(2), await Ok<int, int>(2).MapError(Stringify).ToSync());
            Assert.Equal(Error<int, string>("error code: 13"), await Error<int, int>(13).MapError(Stringify).ToSync());
        }

        [Fact]
        public void GetEnumeratorTests()
        {
            Assert.Equal(new[] { 7 }, Ok<int, string>(7));
            Assert.Empty(Error<int, string>("nothing!"));
        }

        [Fact]
        public void AndTests()
        {
            Assert.Equal(Error<string, string>("late error"), Ok<int, string>(2).And(Error<string, string>("late error")));
            Assert.Equal(Error<string, string>("early error"), Error<int, string>("early error").And(Ok<string, string>("foo")));
            Assert.Equal(Error<string, string>("not a 2"), Error<int, string>("not a 2").And(Error<string, string>("late error")));
            Assert.Equal(Ok<string, string>("different result type"), Ok<int, string>(2).And(Ok<string, string>("different result type")));
        }

        [Fact]
        public void AndThenTests()
        {
            Result<int, int> Square(int x) => Ok<int, int>(x * x);
            Result<int, int> Error(int x) => Error<int, int>(x);

            Assert.Equal(Ok<int, int>(16), Ok<int, int>(2).AndThen(Square).AndThen(Square));
            Assert.Equal(Error<int, int>(4), Ok<int, int>(2).AndThen(Square).AndThen(Error));
            Assert.Equal(Error<int, int>(2), Ok<int, int>(2).AndThen(Error).AndThen(Square));
            Assert.Equal(Error<int, int>(3), Error<int, int>(3).AndThen(Square).AndThen(Square));
        }

        [Fact]
        public void OrTests()
        {
            Assert.Equal(Ok<int, string>(2), Ok<int, string>(2).Or(Error<int, string>("late error")));
            Assert.Equal(Ok<int, string>(2), Error<int, string>("early error").Or(Ok<int, string>(2)));
            Assert.Equal(Error<int, string>("late error"), Error<int, string>("not a 2").Or(Error<int, string>("late error")));
            Assert.Equal(Ok<int, string>(2), Ok<int, string>(2).Or(Ok<int, string>(200)));
        }

        [Fact]
        public void OrElseTests()
        {
            Result<int, int> Square(int x) => Ok<int, int>(x * x);
            Result<int, int> Error(int x) => Error<int, int>(x);

            Assert.Equal(Ok<int, int>(2), Ok<int, int>(2).OrElse(x => Square(x)).OrElse(x => Square(x)));
            Assert.Equal(Ok<int, int>(2), Ok<int, int>(2).OrElse(x => Error(x)).OrElse(x => Square(x)));
            Assert.Equal(Ok<int, int>(9), Error<int, int>(3).OrElse(x => Square(x)).OrElse(x => Error(x)));
            Assert.Equal(Error<int, int>(3), Error<int, int>(3).OrElse(x => Error(x)).OrElse(x => Error(x)));
        }

        [Fact]
        public void UnwrapOrTests()
        {
            const int optionB = 2;
            Assert.Equal(9, Ok<int, string>(9).UnwrapOr(optionB));
            Assert.Equal(optionB, Error<int, string>("error").UnwrapOr(optionB));
        }

        [Fact]
        public void UnwrapOrElseTests()
        {
            int Count(string x) => x.Length;

            Assert.Equal(2, Ok<int, string>(2).UnwrapOrElse(Count));
            Assert.Equal(3, Error<int, string>("foo").UnwrapOrElse(Count));
        }

        [Fact]
        public void UnwrapTests()
        {
            Assert.Equal(2, Ok<int, string>(2).Unwrap());
            var exception = Assert.Throws<RlxException>(() => Error<int, string>("emergency failure").Unwrap());
            Assert.Equal("emergency failure", exception.Message);
        }

        [Fact]
        public void ExpectTests()
        {
            var exception = Assert.Throws<RlxException>(() => Error<int, string>("emergency failure").Expect("Testing expect"));
            Assert.Equal("Testing expect: emergency failure", exception.Message);
        }

        [Fact]
        public void UnwrapErrorTests()
        {
            var exception = Assert.Throws<RlxException>(() => Ok<int, string>(2).UnwrapError());
            Assert.Equal("2", exception.Message);
            Assert.Equal("emergency failure", Error<int, string>("emergency failure").UnwrapError());
        }

        [Fact]
        public void ExpectErrorTests()
        {
            var exception = Assert.Throws<RlxException>(() => Ok<int, string>(10).ExpectError("Testing expect_err"));
            Assert.Equal("Testing expect_err: 10", exception.Message);
        }

        [Fact]
        public void UnwrapOrDefaultTests()
        {
            string goodYearFromInput = "1909";
            string badYearFromInput = "190blarg";
            int goodYear = Parse.Int32(goodYearFromInput).OkOr("Invalid number").UnwrapOrDefault();
            int badYear = Parse.Int32(badYearFromInput).OkOr("Invalid number").UnwrapOrDefault();
            Assert.Equal(1909, goodYear);
            Assert.Equal(0, badYear);
        }

        [Fact]
        public void UnwrapEitherTests()
        {
            var left = Ok<string, string>("abc").UnwrapEither();
            var right = Error<string, string>("abc").UnwrapEither();
            Assert.Equal(left, right);
        }

        [Fact]
        public async Task UnwrapEitherAsyncTests()
        {
            var left = await Ok<string, string>(Task.FromResult("abc")).UnwrapEitherAsync();
            var right = await Error<string, string>(Task.FromResult("abc")).UnwrapEitherAsync();
            Assert.Equal(left, right);
        }
    }
}
