using Rlx;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
    public static class OptionExtensions
    {
        public static IActionResult ToActionResult<T>(this Option<T> option)
            => option.MapOrElse<IActionResult>(() => new NotFoundResult(), value => new OkObjectResult(value));
        
        public static Task<IActionResult> ToActionResult<T>(this OptionTask<T> option)
            => option.MapOrElse<IActionResult>(() => new NotFoundResult(), value => new OkObjectResult(value));
    }
}
