using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Rlx.Tests
{
    public class ParserTests
    {
        public static IEnumerable<object[]> Data
        {
            get => new[] { "a", "1", "one", "234", "123,456" }.Select(param => new object[] { param });
        }

        [MemberData(nameof(Data))]
        [Theory]
        public void Parse_ShouldReturnSomeGivenTruthyResult(string s)
        {
            var option = Parser.Parse<int>(s, int.TryParse);
            bool result = int.TryParse(s, out int value);

            Assert.Equal(result, option.IsSome);
            Assert.Equal(value, option.UnwrapOrDefault());
        }

        [MemberData(nameof(Data))]
        [Theory]
        public void Compile_ShouldReturnDelegateMatchingParse(string s)
        {
            var fn = Parser.Compile<int>(int.TryParse);
            var expected = Parser.Parse<int>(s, int.TryParse);
            var actual = fn(s);
            Assert.Equal(expected, actual);
        }
    }
}