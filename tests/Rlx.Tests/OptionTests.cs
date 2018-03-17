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
            Assert.Equal("value", Some("value").Expect("the world is ending"));
            var exception = Assert.Throws<RlxException>(() => None<string>().Expect("the world is ending"));
            Assert.Equal("the world is ending", exception.Message);
        }

        [Fact]
        public void UnwrapTests()
        {
            Assert.Equal("air", Some("air").Unwrap());
            Assert.Throws<RlxException>(() => None<string>().Unwrap());
        }

        [Fact]
        public void UnwrapOrTests()
        {
            Assert.Equal("car", Some("car").UnwrapOr("bike"));
            Assert.Equal("bike", None<string>().UnwrapOr("bike"));
        }

        [Fact]
        public void UnwrapOrElseTests()
        {
            const int k = 10;
            Assert.Equal(4, Some(4).UnwrapOrElse(() => 2 * k));
            Assert.Equal(20, None<int>().UnwrapOrElse(() => 2 * k));
        }

        [Fact]
        public void MapTests()
        {
            var maybeSomeString = Some("Hello, World!");
            var maybeSomeLength = maybeSomeString.Map(s => s.Length);
            Assert.Equal(Some(13), maybeSomeLength);
        }

        [Fact]
        public async Task MapAsyncTests()
        {
            var maybeSomeString = Some("Hello, World!");
            var maybeSomeLength = maybeSomeString.Map(s => Task.FromResult(s.Length));
            Assert.Equal(Some(13), await maybeSomeLength.ToSync());
        }

        [Fact]
        public void MapOrTests()
        {
            Assert.Equal(3, Some("foo").MapOr(42, v => v.Length));
            Assert.Equal(42, None<string>().MapOr(42, v => v.Length));
        }

        [Fact]
        public void MapOrElseTests()
        {
            const int k = 21;
            Assert.Equal(3, Some("foo").MapOrElse(() => 2 * k, v => v.Length));
            Assert.Equal(42, None<string>().MapOrElse(() => 2 * k, v => v.Length));
        }

        [Fact]
        public async Task MapOrElseAsyncTests()
        {
            const int k = 21;
            Assert.Equal(3, await Some("Foo").MapOrElse(() => Task.FromResult(2 * k), v => Task.FromResult(v.Length)));
            Assert.Equal(42, await None<string>().MapOrElse(() => Task.FromResult(2 * k), v => Task.FromResult(v.Length)));
        }

        [Fact]
        public void OkOrTests()
        {
            Assert.Equal(Ok<string, int>("foo"), Some("foo").OkOr(0));
            Assert.Equal(Error<string, int>(0), None<string>().OkOr(0));
        }

        [Fact]
        public void OkOrElseTests()
        {
            Assert.Equal(Ok<string, int>("foo"), Some("foo").OkOrElse(() => 0));
            Assert.Equal(Error<string, int>(0), None<string>().OkOrElse(() => 0));
        }

        [Fact]
        public void GetEnumeratorTests()
        {
            Assert.Equal(new[] { "foo" }, Some("foo"));
            Assert.Empty(None<string>());
        }

        [Fact]
        public void AndTests()
        {
            Assert.Equal(None<int>(), Some(2).And(None<int>()));
            Assert.Equal(None<string>(), None<string>().And(Some("foo")));
            Assert.Equal(Some("foo"), Some(2).And(Some("foo")));
            Assert.Equal(None<string>(), None<int>().And(None<string>()));
        }

        [Fact]
        public void AndThenTests()
        {
            Option<int> Square(int x) => Some(x * x);
            Option<int> Nope(int _) => None<int>();

            Assert.Equal(Some(16), Some(2).AndThen(Square).AndThen(Square));
            Assert.Equal(None<int>(), Some(2).AndThen(Square).AndThen(Nope));
            Assert.Equal(None<int>(), Some(2).AndThen(Nope).AndThen(Square));
            Assert.Equal(None<int>(), None<int>().AndThen(Square).AndThen(Square));
        }

        [Fact]
        public void OrTests()
        {
            Assert.Equal(Some(2), Some(2).Or(None<int>()));
            Assert.Equal(Some(100), None<int>().Or(Some(100)));
            Assert.Equal(Some(2), Some(2).Or(Some(100)));
            Assert.Equal(None<int>(), None<int>().Or(None<int>()));
        }

        [Fact]
        public void OrElseTests()
        {
            Option<string> Nobody() => None<string>();
            Option<string> Vikings() => Some("vikings");

            Assert.Equal(Some("barbarians"), Some("barbarians").OrElse(() => Vikings()));
            Assert.Equal(Some("vikings"), None<string>().OrElse(() => Vikings()));
            Assert.Equal(None<string>(), None<string>().OrElse(() => Nobody()));
        }

        [Fact]
        public void UnwrapOrDefaultTests()
        {
            string goodYearFromInput = "1909";
            string badYearFromInput = "190blarg";
            int goodYear = Parse.Int32(goodYearFromInput).OkOr("Invalid number").Ok().UnwrapOrDefault();
            int badYear = Parse.Int32(badYearFromInput).OkOr("Invalid number").Ok().UnwrapOrDefault();
            Assert.Equal(1909, goodYear);
            Assert.Equal(0, badYear);
        }

        [Fact]
        public async Task ConsumeTests()
        {
            bool run = false;
            var result = Some(1).Consume(i =>
            {
                Assert.Equal(1, i);
                run = true;
            });
            Assert.Equal(Some(Unit.Value), result);
            Assert.True(run);

            run = false;
            result = await Some(2).Consume(i =>
            {
                Assert.Equal(2, i);
                run = true;
                return Task.CompletedTask;
            }).ToSync();
            Assert.Equal(Some(Unit.Value), result);
            Assert.True(run);

            run = false;
            result = await Some(Task.FromResult(3)).Consume(i =>
            {
                Assert.Equal(3, i);
                run = true;
            }).ToSync();
            Assert.Equal(Some(Unit.Value), result);
            Assert.True(run);

            run = false;
            result = await Some(Task.FromResult(4)).Consume(i =>
            {
                Assert.Equal(4, i);
                run = true;
                return Task.CompletedTask;
            }).ToSync();
            Assert.Equal(Some(Unit.Value), result);
            Assert.True(run);

            run = false;
            result = None<int>().Consume(i =>
            {
                run = true;
            });
            Assert.Equal(None<Unit>(), result);
            Assert.False(run);

            run = false;
            result = await None<int>().Consume(i =>
            {
                run = true;
                return Task.CompletedTask;
            }).ToSync();
            Assert.Equal(None<Unit>(), result);
            Assert.False(run);

            run = false;
            result = await NoneAsync<int>().Consume(i =>
            {
                run = true;
            }).ToSync();
            Assert.Equal(None<Unit>(), result);
            Assert.False(run);

            run = false;
            result = await NoneAsync<int>().Consume(i =>
            {
                run = true;
                return Task.CompletedTask;
            }).ToSync();
            Assert.Equal(None<Unit>(), result);
            Assert.False(run);
        }
    }
}
