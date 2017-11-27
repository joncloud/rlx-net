using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Rlx.Functions;

namespace Rlx.Tests
{
    public class OptionTaskTests
    {
        Task<T> T<T>(T value)
            => Task.FromResult(value);
        
        [Fact]
        public async Task ExpectTests()
        {
            Assert.Equal("value", await Some(T("value")).ExpectAsync("the world is ending"));
            var exception = await Assert.ThrowsAsync<RlxException>(() => NoneAsync<string>().ExpectAsync("the world is ending"));
            Assert.Equal("the world is ending", exception.Message);
        }

        [Fact]
        public async Task UnwrapTests()
        {
            Assert.Equal("air", await Some(T("air")).UnwrapAsync());
            await Assert.ThrowsAsync<RlxException>(() => NoneAsync<string>().UnwrapAsync());
        }

        [Fact]
        public async Task UnwrapOrTests()
        {
            Assert.Equal("car", await Some(T("car")).UnwrapOrAsync("bike"));
            Assert.Equal("bike", await NoneAsync<string>().UnwrapOrAsync("bike"));
        }

        [Fact]
        public async Task UnwrapOrDefaultTests()
        {
            string goodYearFromInput = "1909";
            string badYearFromInput = "190blarg";
            int goodYear = await Parse.Int32(goodYearFromInput).OkOr("Invalid number").Map(x => Task.FromResult(x)).Ok().UnwrapOrDefaultAsync();
            int badYear = await Parse.Int32(badYearFromInput).OkOr("Invalid number").Map(x => Task.FromResult(x)).Ok().UnwrapOrDefaultAsync();
            Assert.Equal(1909, goodYear);
            Assert.Equal(0, badYear);
        }

        [Fact]
        public async Task MapTests()
        {
            var maybeSomeString = Some(T("Hello, World!"));
            var maybeSomeLength = maybeSomeString.Map(s => s.Length);
            Assert.Equal(Some(13), await maybeSomeLength.ToSync());
        }

        [Fact]
        public async Task MapOrElseTests()
        {
            const int k = 21;
            Assert.Equal(3, await Some(T("foo")).MapOrElse(() => T(2 * k), v => T(v.Length)));
            Assert.Equal(42, await NoneAsync<string>().MapOrElse(() => T(2 * k), v => T(v.Length)));
        }

        [Fact]
        public async Task AndTests()
        {
            Assert.Equal(None<int>(), await Some(T(2)).And(NoneAsync<int>()).ToSync());
            Assert.Equal(None<string>(), await NoneAsync<string>().And(Some(T("foo"))).ToSync());
            Assert.Equal(Some("foo"), await Some(T(2)).And(Some(T("foo"))).ToSync());
            Assert.Equal(None<string>(), await NoneAsync<int>().And(NoneAsync<string>()).ToSync());
        }

        [Fact]
        public async Task OrTests()
        {
            Assert.Equal(Some(2), await Some(T(2)).Or(NoneAsync<int>()).ToSync());
            Assert.Equal(Some(100), await NoneAsync<int>().Or(Some(T(100))).ToSync());
            Assert.Equal(Some(2), await Some(T(2)).Or(Some(T(100))).ToSync());
            Assert.Equal(None<int>(), await NoneAsync<int>().Or(NoneAsync<int>()).ToSync());
        }
    }
}
