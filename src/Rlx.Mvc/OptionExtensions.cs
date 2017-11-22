using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Rlx.Mvc
{
    public static class OptionExtensions
    {
        public static IActionResult ToActionResult<T>(this Option<T> option)
            => option.MapOrElse<IActionResult>(() => new NotFoundResult(), value => new OkObjectResult(value));

        public static Task<IActionResult> ToActionResult<T>(this OptionTask<T> option)
            => option.MapOrElse<IActionResult>(() => new NotFoundResult(), value => new OkObjectResult(value));
    }
}
