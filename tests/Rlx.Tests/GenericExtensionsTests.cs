using Xunit;
using static Rlx.Functions;

namespace Rlx.Tests
{
    public class GenericExtensionsTests
    {
        [Fact]
        public void ClassTests()
        {
            string s = "abc";
            Assert.Equal(Some("abc"), s.ToOption());
            s = null;
            Assert.Equal(None<string>(), s.ToOption());
        }

        [Fact]
        public void StructTests()
        {
            int? i = 123;
            Assert.Equal(Some(123), i.ToOption());
            i = null;
            Assert.Equal(None<int>(), i.ToOption());
        }
    }
}
