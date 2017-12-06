using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;
using static Rlx.Functions;

namespace Rlx.MvcCore.Tests
{
    public class ResultExtensionsTests
    {
        [Fact]
        public void OkTests()
        {
            var result = Ok<int, string>(123);
            var actionResult = result.ToActionResult();
            var objectResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(123, objectResult.Value);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            actionResult = result.ToActionResult(x => x * 2, _ => 0);
            objectResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(123, objectResult.Value);
            Assert.Equal(246, objectResult.StatusCode);
        }

        [Fact]
        public async Task OkAsyncTests()
        {
            var result = Ok<int, string>(Task.FromResult(123));
            var actionResult = await result.ToActionResult();
            var objectResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(123, objectResult.Value);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            actionResult = await result.ToActionResult(x => x * 2, _ => 0);
            objectResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(123, objectResult.Value);
            Assert.Equal(246, objectResult.StatusCode);
        }

        [Fact]
        public void ErrorTests()
        {
            var result = Error<int, string>("bad news!123");
            var actionResult = result.ToActionResult();
            var statusCodeResult = Assert.IsAssignableFrom<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);

            actionResult = result.ToActionResult(_ => 0, x => int.Parse(x.Split('!')[1]));
            statusCodeResult = Assert.IsAssignableFrom<StatusCodeResult>(actionResult);
            Assert.Equal(123, statusCodeResult.StatusCode);

            actionResult = result.ToActionResult(_ => 0, _ => 400, x => Some(x));
            var objectResult = Assert.IsAssignableFrom<ObjectResult>(actionResult);
            Assert.Equal(400, objectResult.StatusCode);
            Assert.Equal("bad news!123", objectResult.Value);
        }

        [Fact]
        public async Task ErrorAsyncTests()
        {
            var result = Error<int, string>(Task.FromResult("bad news!123"));
            var actionResult = await result.ToActionResult();
            var statusCodeResult = Assert.IsAssignableFrom<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);

            actionResult = await result.ToActionResult(_ => 0, x => int.Parse(x.Split('!')[1]));
            statusCodeResult = Assert.IsAssignableFrom<StatusCodeResult>(actionResult);
            Assert.Equal(123, statusCodeResult.StatusCode);

            actionResult = await result.ToActionResult(_ => 0, _ => 400, x => Some(x));
            var objectResult = Assert.IsAssignableFrom<ObjectResult>(actionResult);
            Assert.Equal(400, objectResult.StatusCode);
            Assert.Equal("bad news!123", objectResult.Value);
        }
    }
}
