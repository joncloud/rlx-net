using System.Threading.Tasks;
using Xunit;
using static Rlx.Functions;

namespace Rlx.Tests
{
    public class OptionTests
    {
        [Fact]
        public void IsSomeTests()
        {
            Assert.True(Some(2).IsSome);
            Assert.False(None<int>().IsSome);
        }

        [Fact]
        public void IsNoneTests()
        {
            Assert.False(Some(2).IsNone);
            Assert.True(None<int>().IsNone);
        }

        [Fact]
        public void ExpectTests()
        {
            Assert.Equal(Some("value").Expect("the world is ending"), "value");
            var exception = Assert.Throws<RlxException>(() => None<string>().Expect("the world is ending"));
            Assert.Equal("the world is ending", exception.Message);
        }

        [Fact]
        public void UnwrapTests()
        {
            Assert.Equal(Some("air").Unwrap(), "air");
            Assert.Throws<RlxException>(() => None<string>().Unwrap());
        }

        [Fact]
        public void UnwrapOrTests()
        {
            Assert.Equal(Some("car").UnwrapOr("bike"), "car");
            Assert.Equal(None<string>().UnwrapOr("bike"), "bike");
        }

        [Fact]
        public void UnwrapOrElseTests()
        {
            const int k = 10;
            Assert.Equal(Some(4).UnwrapOrElse(() => 2 * k), 4);
            Assert.Equal(None<int>().UnwrapOrElse(() => 2 * k), 20);
        }

        [Fact]
        public void MapTests()
        {
            var maybeSomeString = Some("Hello, World!");
            var maybeSomeLength = maybeSomeString.Map(s => s.Length);
            Assert.Equal(maybeSomeLength, Some(13));
        }

        [Fact]
        public async Task MapAsyncTests()
        {
            var maybeSomeString = Some("Hello, World!");
            var maybeSomeLength = maybeSomeString.Map(s => Task.FromResult(s.Length));
            Assert.Equal(await maybeSomeLength.ToSync(), Some(13));
        }

        [Fact]
        public void MapOrTests()
        {
            Assert.Equal(Some("foo").MapOr(42, v => v.Length), 3);
            Assert.Equal(None<string>().MapOr(42, v => v.Length), 42);
        }

        [Fact]
        public void MapOrElseTests()
        {
            const int k = 21;
            Assert.Equal(Some("foo").MapOrElse(() => 2 * k, v => v.Length), 3);
            Assert.Equal(None<string>().MapOrElse(() => 2 * k, v => v.Length), 42);
        }

        [Fact]
        public async Task MapOrElseAsyncTests()
        {
            const int k = 21;
            Assert.Equal(await Some("Foo").MapOrElse(() => Task.FromResult(2 * k), v => Task.FromResult(v.Length)), 3);
            Assert.Equal(await None<string>().MapOrElse(() => Task.FromResult(2 * k), v => Task.FromResult(v.Length)), 42);
        }

        [Fact]
        public void OkOrTests()
        {
            Assert.Equal(Some("foo").OkOr(0), Ok<string, int>("foo"));
            Assert.Equal(None<string>().OkOr(0), Error<string, int>(0));
        }

        [Fact]
        public void OkOrElseTests()
        {
            Assert.Equal(Some("foo").OkOrElse(() => 0), Ok<string, int>("foo"));
            Assert.Equal(None<string>().OkOrElse(() => 0), Error<string, int>(0));
        }

        [Fact]
        public void GetEnumeratorTests()
        {
            Assert.Equal(Some("foo"), new[] { "foo" });
            Assert.Empty(None<string>());
        }

        [Fact]
        public void AndTests()
        {
            Assert.Equal(Some(2).And(None<int>()), None<int>());
            Assert.Equal(None<string>().And(Some("foo")), None<string>());
            Assert.Equal(Some(2).And(Some("foo")), Some("foo"));
            Assert.Equal(None<int>().And(None<string>()), None<string>());
        }

        [Fact]
        public void AndThenTests()
        {
            Option<int> Square(int x) => Some(x * x);
            Option<int> Nope(int _) => None<int>();

            Assert.Equal(Some(2).AndThen(Square).AndThen(Square), Some(16));
            Assert.Equal(Some(2).AndThen(Square).AndThen(Nope), None<int>());
            Assert.Equal(Some(2).AndThen(Nope).AndThen(Square), None<int>());
            Assert.Equal(None<int>().AndThen(Square).AndThen(Square), None<int>());
        }

        [Fact]
        public void OrTests()
        {
            Assert.Equal(Some(2).Or(None<int>()), Some(2));
            Assert.Equal(None<int>().Or(Some(100)), Some(100));
            Assert.Equal(Some(2).Or(Some(100)), Some(2));
            Assert.Equal(None<int>().Or(None<int>()), None<int>());
        }

        [Fact]
        public void OrElseTests()
        {
            Option<string> Nobody() => None<string>();
            Option<string> Vikings() => Some("vikings");

            Assert.Equal(Some("barbarians").OrElse(() => Vikings()), Some("barbarians"));
            Assert.Equal(None<string>().OrElse(() => Vikings()), Some("vikings"));
            Assert.Equal(None<string>().OrElse(() => Nobody()), None<string>());
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
            int goodYear = Parse(goodYearFromInput).Ok().UnwrapOrDefault();
            int badYear = Parse(badYearFromInput).Ok().UnwrapOrDefault();
            Assert.Equal(1909, goodYear);
            Assert.Equal(0, badYear);
        }
    }
}
