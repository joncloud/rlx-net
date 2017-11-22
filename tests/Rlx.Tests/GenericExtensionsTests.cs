using System.Threading.Tasks;
using Xunit;
using static Rlx.Functions;

namespace Rlx.Tests
{
    public class GenericExtensionsTests
    {
        [Fact]
        public async Task ClassTests()
        {
            string s = "abc";
            Assert.Equal(Some("abc"), s.ToOption());
            Assert.Equal(Some("abc"), await Task.FromResult(s).ToOption().ToSync());

            s = null;
            Assert.Equal(None<string>(), s.ToOption());
            Assert.Equal(None<string>(), await Task.FromResult(s).ToOption().ToSync());
        }

        [Fact]
        public async Task StructTests()
        {
            int i = 123;
            Assert.Equal(Some(123), i.ToOption());
            Assert.Equal(Some(123), await Task.FromResult(i).ToOption().ToSync());

            i = 0;
            Assert.Equal(Some(0), i.ToOption());
            Assert.Equal(Some(0), await Task.FromResult(i).ToOption().ToSync());
        }

        [Fact]
        public async Task NullableStructTests()
        {
            int? i = 123;
            Assert.Equal(Some(123), i.ToOption());
            Assert.Equal(Some(123), await Task.FromResult(i).ToOption().ToSync());

            i = null;
            Assert.Equal(None<int>(), i.ToOption());
            Assert.Equal(None<int>(), await Task.FromResult(i).ToOption().ToSync());
        }
    }
}
