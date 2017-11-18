using System;
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
            Assert.Equal(Ok<int, string>(2).Ok(), Some(2));
            Assert.Equal(Error<int, string>("Nothing here").Ok(), None<int>());
        }

        [Fact]
        public void ErrorTests()
        {
            Assert.Equal(Ok<int, string>(2).Error(), None<string>());
            Assert.Equal(Error<int, string>("Nothing here").Error(), Some("Nothing here"));
        }

        [Fact]
        public void MapTests()
        {
            Result<int, string> Parse(string s)
                => int.TryParse(s, out var r)
                ? Ok<int, string>(r)
                : Error<int, string>("Invalid number");

            const string line = "1\n2\n3\n4\n";
            var values = new[] {
                Ok<int, string>(2),
                Ok<int, string>(4),
                Ok<int, string>(6),
                Ok<int, string>(8)
            };
            int index = 0;
            var nums = line.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var num in nums)
            {
                Assert.Equal(Parse(num).Map(i => i * 2), values[index++]);
            }
        }

        [Fact]
        public async Task MapAsyncTests()
        {
            Result<int, string> Parse(string s)
                   => int.TryParse(s, out var r)
                   ? Ok<int, string>(r)
                   : Error<int, string>("Invalid number");

            const string line = "1\n2\n3\n4\n";
            var values = new[] {
                Ok<int, string>(2),
                Ok<int, string>(4),
                Ok<int, string>(6),
                Ok<int, string>(8)
            };
            int index = 0;
            var nums = line.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var num in nums)
            {
                Assert.Equal(await Parse(num).Map(i => Task.FromResult(i * 2)).ToSync(), values[index++]);
            }
        }

        [Fact]
        public void MapErrorTests()
        {
            string Stringify(int x) => $"error code: {x}";

            Assert.Equal(Ok<int, int>(2).MapError(Stringify), Ok<int, string>(2));
            Assert.Equal(Error<int, int>(13).MapError(Stringify), Error<int, string>("error code: 13"));
        }

        [Fact]
        public async Task MapErrorAsyncTests()
        {
            Task<string> Stringify(int x) => Task.FromResult($"error code: {x}");

            Assert.Equal(await Ok<int, int>(2).MapError(Stringify).ToSync(), Ok<int, string>(2));
            Assert.Equal(await Error<int, int>(13).MapError(Stringify).ToSync(), Error<int, string>("error code: 13"));
        }

        [Fact]
        public void GetEnumeratorTests()
        {
            Assert.Equal(Ok<int, string>(7), new[] { 7 });
            Assert.Empty(Error<int, string>("nothing!"));
        }

        [Fact]
        public void AndTests()
        {
            Assert.Equal(Ok<int, string>(2).And(Error<string, string>("late error")), Error<string, string>("late error"));
            Assert.Equal(Error<int, string>("early error").And(Ok<string, string>("foo")), Error<string, string>("early error"));
            Assert.Equal(Error<int, string>("not a 2").And(Error<string, string>("late error")), Error<string, string>("not a 2"));
            Assert.Equal(Ok<int, string>(2).And(Ok<string, string>("different result type")), Ok<string, string>("different result type"));
        }

        [Fact]
        public void AndThenTests()
        {
            Result<int, int> Square(int x) => Ok<int, int>(x * x);
            Result<int, int> Error(int x) => Error<int, int>(x);

            Assert.Equal(Ok<int, int>(2).AndThen(Square).AndThen(Square), Ok<int, int>(16));
            Assert.Equal(Ok<int, int>(2).AndThen(Square).AndThen(Error), Error<int, int>(4));
            Assert.Equal(Ok<int, int>(2).AndThen(Error).AndThen(Square), Error<int, int>(2));
            Assert.Equal(Error<int, int>(3).AndThen(Square).AndThen(Square), Error<int, int>(3));
        }

        [Fact]
        public void OrTests()
        {
            Assert.Equal(Ok<int, string>(2).Or(Error<int, string>("late error")), Ok<int, string>(2));
            Assert.Equal(Error<int, string>("early error").Or(Ok<int, string>(2)), Ok<int, string>(2));
            Assert.Equal(Error<int, string>("not a 2").Or(Error<int, string>("late error")), Error<int, string>("late error"));
            Assert.Equal(Ok<int, string>(2).Or(Ok<int, string>(200)), Ok<int, string>(2));
        }

        [Fact]
        public void OrElseTests()
        {
            Result<int, int> Square(int x) => Ok<int, int>(x * x);
            Result<int, int> Error(int x) => Error<int, int>(x);

            Assert.Equal(Ok<int, int>(2).OrElse(Square).OrElse(Square), Ok<int, int>(2));
            Assert.Equal(Ok<int, int>(2).OrElse(Error).OrElse(Square), Ok<int, int>(2));
            Assert.Equal(Error<int, int>(3).OrElse(Square).OrElse(Error), Ok<int, int>(9));
            Assert.Equal(Error<int, int>(3).OrElse(Error).OrElse(Error), Error<int, int>(3));
        }

        [Fact]
        public void UnwrapOrTests()
        {
            const int optionB = 2;
            Assert.Equal(Ok<int, string>(9).UnwrapOr(optionB), 9);
            Assert.Equal(Error<int, string>("error").UnwrapOr(optionB), optionB);
        }

        [Fact]
        public void UnwrapOrElseTests()
        {
            int Count(string x) => x.Length;

            Assert.Equal(Ok<int, string>(2).UnwrapOrElse(Count), 2);
            Assert.Equal(Error<int, string>("foo").UnwrapOrElse(Count), 3);
        }

        [Fact]
        public void UnwrapTests()
        {
            Assert.Equal(Ok<int, string>(2).Unwrap(), 2);
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
            Assert.Equal(Error<int, string>("emergency failure").UnwrapError(), "emergency failure");
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
            Result<int, string> Parse(string s)
                => int.TryParse(s, out var r)
                ? Ok<int, string>(r)
                : Error<int, string>("Invalid number");

            string goodYearFromInput = "1909";
            string badYearFromInput = "190blarg";
            int goodYear = Parse(goodYearFromInput).UnwrapOrDefault();
            int badYear = Parse(badYearFromInput).UnwrapOrDefault();
            Assert.Equal(1909, goodYear);
            Assert.Equal(0, badYear);
        }
    }
}
