using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;
using static Rlx.Functions;

namespace Rlx.MvcCore.Tests
{
    public class OptionExtensionsTests
    {
        [Fact]
        public void SomeTests()
        {
            var option = Some(123);
            var actionResult = option.ToActionResult();
            var objectResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(123, objectResult.Value);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            actionResult = option.ToActionResult(456);
            objectResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(123, objectResult.Value);
            Assert.Equal(456, objectResult.StatusCode);
        }

        [Fact]
        public async Task SomeAsyncTests()
        {
            var option = Some(Task.FromResult(123));
            var actionResult = await option.ToActionResult();
            var objectResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(123, objectResult.Value);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            actionResult = await option.ToActionResult(456);
            objectResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(123, objectResult.Value);
            Assert.Equal(456, objectResult.StatusCode);
        }

        [Fact]
        public void NoneTests()
        {
            var option = None<int>();
            var actionResult = option.ToActionResult();
            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);

            actionResult = option.ToActionResult(456);
            notFoundResult = Assert.IsType<NotFoundResult>(actionResult);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task NoneAsyncTests()
        {
            var option = NoneAsync<int>();
            var actionResult = await option.ToActionResult();
            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);

            actionResult = await option.ToActionResult(456);
            notFoundResult = Assert.IsType<NotFoundResult>(actionResult);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }
    }
}
