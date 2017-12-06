using Microsoft.AspNetCore.Http;
using Rlx;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
    public static class OptionExtensions
    {
        public static IActionResult ToActionResult<T>(this Option<T> option) =>
            option.ToActionResult(StatusCodes.Status200OK);

        public static IActionResult ToActionResult<T>(this Option<T> option, int statusCode) =>
            option.MapOrElse<IActionResult>(() => new NotFoundResult(), value => new ObjectResult(value) { StatusCode = statusCode });

        public static Task<IActionResult> ToActionResult<T>(this OptionTask<T> option) =>
            option.ToActionResult(StatusCodes.Status200OK);

        public static Task<IActionResult> ToActionResult<T>(this OptionTask<T> option, int statusCode) =>
            option.MapOrElse<IActionResult>(() => new NotFoundResult(), value => new ObjectResult(value) { StatusCode = statusCode });
    }
}
