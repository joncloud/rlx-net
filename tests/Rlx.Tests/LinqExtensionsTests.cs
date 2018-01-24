using System;
using System.Linq;
using Xunit;

namespace Rlx.Tests
{
    public class LinqExtensionsTests
    {
        [Fact]
        public void EnumerableFirstOrOption_ShouldReturnNoneGivenEmptySource()
        {
            var option = Array.Empty<int>().FirstOrOption();
            Assert.True(option.IsNone);
        }

        [InlineData(1)]
        [InlineData(2)]
        [Theory]
        public void EnumerableFirstOrOption_ShouldReturnSomeGivenNonEmptySource(int count)
        {
            var enumerable = Enumerable.Range(1, count);
            var option = enumerable.FirstOrOption();
            Assert.True(option.IsSome);
            Assert.Equal(enumerable.First(), option.Unwrap());
        }

        [Fact]
        public void QueryableFirstOrOption_ShouldReturnNoneGivenEmptySource()
        {
            var enumerable = Array.Empty<int>();
            var query = enumerable.AsQueryable();

            var left = enumerable.FirstOrOption();
            var right = query.FirstOrOption();
            Assert.Equal(left, right);
        }

        [InlineData(1)]
        [InlineData(2)]
        [Theory]
        public void QueryableFirstOrOption_ShouldReturnSomeGivenNonEmptySource(int count)
        {
            var enumerable = Enumerable.Range(1, count);
            var query = enumerable.AsQueryable();

            var left = enumerable.FirstOrOption();
            var right = query.FirstOrOption();
            Assert.Equal(left, right);
        }
    }
}
